using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Radin.Fraud.Identity.Data.Configs;
using Radin.Fraud.Identity.Data.Entities;

namespace Radin.Fraud.Identity.Data
{
	public class FraudIdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
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
		public DbSet<ApplicationUser> Users { get; set; }
		public DbSet<ApplicationRole> Roles { get; set; }
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
						optionsBuilder.UseSqlServer(connectionString);/*, options =>*/
						//options.MigrationsAssembly("Migrations.SQL"));
					}
				}
			}
		}
	}
}
