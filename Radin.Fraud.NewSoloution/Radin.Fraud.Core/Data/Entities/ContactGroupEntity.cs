
using Radin.Fraud.Core.Data.Domain;
using System.Text.Json.Serialization;

namespace Radin.Fraud.Core.Data.Entities
{
	public class ContactGroupEntity : ContactGroup
	{
		//public override string GetTableName() => "ContactGroup";
		[JsonIgnore]
		public virtual ICollection<ContactGroupContactEntity> ContactGroupContacts { get; set; } = new HashSet<ContactGroupContactEntity>();
		[JsonIgnore]
		public virtual ICollection<AlertContactGroupEntity> AlertContactGroups { get; set; }
	}
}
