using BackEndInfrastructure.DynamicLinqCore;
using BackEndInfrastructure.Infrastructure.Service;
using Radin.Fraud.Core.Data.Domain;
using Radin.Fraud.Core.Infrastructure.UnitOfWork;

namespace Radin.Fraud.Core.Services
{
	public class AlertService : StorageBusinessService<Alert, int>
	{
		private readonly ICoreUnitOfWork _unitOfWork;
		private const int _serviceLogNumber = 100;
		public AlertService(ICoreUnitOfWork coreUnitOfWork, ILogger<Alert> logger) : base(logger, _serviceLogNumber)
		{
			_unitOfWork = coreUnitOfWork;
		}

		public override Task<int> AddAsync(Alert item)
		{
			throw new NotImplementedException();
		}

		public override Task<LinqDataResult<Alert>> ItemsAsync(LinqDataRequest request)
		{
			throw new NotImplementedException();
		}

		public override Task ModifyAsync(Alert item)
		{
			throw new NotImplementedException();
		}

		public override Task RemoveByIdAsync(int ID)
		{
			throw new NotImplementedException();
		}

		public override Task<Alert> RetrieveByIdAsync(int ID)
		{
			throw new NotImplementedException();
		}

		protected override Task ValidateOnAddAsync(Alert item)
		{
			throw new NotImplementedException();
		}

		protected override Task ValidateOnModifyAsync(Alert recievedItem, Alert storageItem)
		{
			throw new NotImplementedException();
		}
	}
}
