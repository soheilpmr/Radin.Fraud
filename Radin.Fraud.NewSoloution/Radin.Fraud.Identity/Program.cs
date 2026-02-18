using BackEndInfrastructure.JwtTransformer;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FraudIdentity.DB.SQL.Data;
using FraudIdentity.DB.SQL.Data.Configs;
using FraudIdentity.DB.SQL.Data.Entities;
using Radin.Fraud.Identity.Services;
using Scalar.AspNetCore;
using Serilog;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.Enrich.FromLogContext()
	.Filter.ByExcluding(logEvent =>
			logEvent.Properties.TryGetValue("RequestPath", out var path) &&
			(path.ToString().Contains("/metrics") || path.ToString().Contains("/health")))
	 .WriteTo.Console()
	.WriteTo.File(
		"./log/fraaudauthservice/fraaudauthservice-.log",
		rollingInterval: RollingInterval.Day,
		shared: true)
	.CreateLogger();

builder.AddServiceDefaults();

// Add services to the container.
#region Log
builder.Host.UseSerilog();
#endregion

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
#region JWT
//builder.Services.AddAuthentication(
//    options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(x =>
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	  .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
	  {
		  options.MapInboundClaims = false;
		  options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
		  {
			  ValidIssuer = builder.Configuration["JWTBearerSettings:Issuer"],
			  ValidAudience = builder.Configuration["JWTBearerSettings:Audience"],
			  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTBearerSettings:Key"])),
			  ValidateIssuerSigningKey = true,
			  ValidateIssuer = true,
			  ValidateAudience = true,
			  ValidateLifetime = true,
			  ClockSkew = TimeSpan.Zero,
			  RoleClaimType = JwtClaimTypes.Role
			  //NameClaimType = JwtClaimTypes.Name
		  };
		  options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
		  {
			  OnTokenValidated = async context =>
			  {
				  var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
				  var user = await userManager.GetUserAsync(context.Principal);

				  if (user == null)
				  {
					  context.Fail("Unauthorized: user not found.");
					  return;
				  }

				  var securityStampClaim = context.Principal.FindFirstValue("AspNet.Identity.SecurityStamp");

				  if (securityStampClaim == null || user.SecurityStamp != securityStampClaim)
				  {
					  context.Fail("This token has been invalidated.");
				  }
			  }
		  };
	  });
#endregion
builder.Services.AddOpenApi(options =>
{
	options.AddDocumentTransformer<JwtBearerTransformer>();
});
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("ApiScope", policy =>
	{
		policy.RequireAuthenticatedUser();
	});
});

builder.Services.AddControllers();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 6;
	options.Password.RequireUppercase = false;
	options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<FraudIdentityDbContext>()
.AddDefaultTokenProviders();

#region Configure
builder.Services.Configure<ConnectionStringConfig>(builder.Configuration.GetSection("ConnectionStrings"));
#endregion

#region SQLServerRegistration
builder.Services.AddDbContext<FraudIdentityDbContext>(options =>
	options.UseSqlServer(
			  builder.Configuration.GetConnectionString("DefaultConnectionSQLServer"),
		sql => sql.MigrationsAssembly("FraudIdentity.DB.SQL")
	));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IJWTService, JWTService>();
#endregion

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.MapOpenApi();
app.MapScalarApiReference("/scalar", options =>
{
	options.Title = "IDGF Auth API";
	options.Theme = ScalarTheme.BluePlanet;
	options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
	.AddApiKeyAuthentication("JWT", options => options.Value = "Bearer");
});
app.MapGet("/", ctx =>
{
	ctx.Response.Redirect("/scalar");
	return Task.CompletedTask;
});

if (app.Environment.EnvironmentName == "Production")
	app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
