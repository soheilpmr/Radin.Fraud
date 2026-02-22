using Microsoft.EntityFrameworkCore;
using Radin.Fraud.Core.Data.Domain;
using Radin.Fraud.Core.Data.Entities;

namespace Radin.Fraud.Core.Data
{
	public class WebAdminDbContext :DbContext
	{
		public WebAdminDbContext(DbContextOptions<WebAdminDbContext> options) : base(options)
		{
			this.ChangeTracker.LazyLoadingEnabled = false;
		}
		public WebAdminDbContext() : base()
		{
			this.ChangeTracker.LazyLoadingEnabled = false;
			//_configuration = configuration;
		}

		public DbSet<AlertEntity> Alerts { get; set; }
	}
}
