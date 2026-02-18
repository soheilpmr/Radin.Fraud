using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using FraudIdentity.DB.SQL.Data.Configs;
using FraudIdentity.DB.SQL.Data.Entities;
using Microsoft.Extensions.Configuration.Json; // (optional, not strictly required)

namespace FraudIdentity.DB.SQL.Data
{
	// Change the base class to use int as the key type, matching ApplicationRole : IdentityRole<int>
	public class FraudIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
	{
		private readonly IServiceProvider _serviceProvider;
		public FraudIdentityDbContext(DbContextOptions<FraudIdentityDbContext> options, IServiceProvider serviceProvider) : base(options)
		{
			_serviceProvider = serviceProvider;
			this.ChangeTracker.LazyLoadingEnabled = false;
		}
		public FraudIdentityDbContext(IServiceProvider serviceProvider) : base()
		{
			_serviceProvider = serviceProvider;
			this.ChangeTracker.LazyLoadingEnabled = false;
		}
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public DbSet<ApplicationRole> ApplicationRoles { get; set; }
		public DbSet<ClaimDefinition> ClaimDefinitions { get; set; }

		private static string getConnectionStringSQLServer()
		{
			var environmentName =
			  Environment.GetEnvironmentVariable(
				  "ASPNETCORE_ENVIRONMENT");

			var config = new ConfigurationBuilder().AddJsonFile("appsettings" + (String.IsNullOrWhiteSpace(environmentName) ? "" : "." + environmentName) + ".json", false).Build();

			return config.GetConnectionString("DefaultConnectionSQLServer");
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				var connectionString = getConnectionStringSQLServer();

				using (var scope = _serviceProvider.CreateScope())
				{
					var connectionStringConfig = scope.ServiceProvider.GetRequiredService<IOptions<ConnectionStringConfig>>().Value;
					if (connectionStringConfig.SQLServerActivaityStatus == "true")
					{
						optionsBuilder.UseSqlServer(connectionString);
					}
				}
			}
		}
	}
}
