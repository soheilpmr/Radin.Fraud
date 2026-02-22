using BackEndInfrastructure.Infrastructure;
using BackEndInfrastructure.Infrastructure.UnitOfWork;
using BackEndInfrsastructure.Domain;
using Radin.Fraud.Core.Data;

namespace Radin.Fraud.Core.Infrastructure.UnitOfWork
{
	public class CoreUnitOfWork : UnitOfWorkAsync<WebAdminDbContext>, ICoreUnitOfWork
	{
		public CoreUnitOfWork()  :base(new WebAdminDbContext())
		{
			
		}

		public ILDRCompatibleRepositoryAsync<T, PrimKey> GetRepo<T, PrimKey>()
			where T : Model<PrimKey>
			where PrimKey : struct
		{
			throw new NotImplementedException();
		}
	}
}
