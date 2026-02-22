using Radin.Fraud.Core.Data.Domain;

namespace Radin.Fraud.Core.Data.Entities
{
	public class ContactGroupContactEntity : ContactGroupContact
	{
		public virtual ContactEntity Contact { get; set; }
		public virtual ContactGroupEntity ContactGroup { get; set; }
	}
}
