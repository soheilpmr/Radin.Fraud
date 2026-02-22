using BackEndInfrastructure.Infrastructure;
using Radin.Fraud.Core.Data.Domain;

namespace Radin.Fraud.Core.Infrastructure.Repositories.Interface
{
	public interface IAlertRepository : ILDRCompatibleRepositoryAsync<Alert, int>
	{

	}
}
