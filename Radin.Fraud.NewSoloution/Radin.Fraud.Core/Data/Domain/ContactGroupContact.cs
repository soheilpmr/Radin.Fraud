using BackEndInfrsastructure.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Radin.Fraud.Core.Data.Domain
{
	public class ContactGroupContact : Model<int>
	{
		public long ContactId { get; set; }
		public long ContactGroupId { get; set; }
	}
}
