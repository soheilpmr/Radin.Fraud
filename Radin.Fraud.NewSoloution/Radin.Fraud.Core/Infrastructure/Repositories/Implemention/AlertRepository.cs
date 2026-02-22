using BackEndInfrastructure.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Radin.Fraud.Core.Data;
using Radin.Fraud.Core.Data.Domain;
using Radin.Fraud.Core.Data.Entities;
using Radin.Fraud.Core.Infrastructure.Repositories.Interface;

namespace Radin.Fraud.Core.Infrastructure.Repositories.Implemention
{
	public class AlertRepository : LDRCompatibleRepositoryAsync<AlertEntity, Alert, int>, IAlertRepository
	{
		private readonly WebAdminDbContext _context;	
		public AlertRepository(WebAdminDbContext context) : base(context)
		{

		}


	}
}
