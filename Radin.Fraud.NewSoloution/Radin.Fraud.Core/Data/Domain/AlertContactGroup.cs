using BackEndInfrsastructure.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Radin.Fraud.Core.Data.Domain
{
	public class AlertContactGroup : Model<int>
	{
		public long AlertId { get; set; }
		public long ContactGroupId { get; set; }
	}
}
