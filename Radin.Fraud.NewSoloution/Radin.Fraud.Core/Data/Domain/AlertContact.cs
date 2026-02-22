using BackEndInfrsastructure.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Radin.Fraud.Core.Data.Domain
{
	public class AlertContact : Model<long>
	{
		public long ContactId { get; set; }
		public long AlertId { get; set; }
	}
}
