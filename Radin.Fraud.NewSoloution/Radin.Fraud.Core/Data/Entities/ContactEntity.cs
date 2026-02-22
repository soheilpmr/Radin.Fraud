

using Radin.Fraud.Core.Data.Domain;
using System.Text.Json.Serialization;

namespace Radin.Fraud.Core.Data.Entities
{
	public class ContactEntity : Contact
	{
		[JsonIgnore]
		public virtual ICollection<AlertContactEntity> AlertContacts { get; set; }//new
		[JsonIgnore]
		public virtual ICollection<ContactGroupContactEntity> ContactGroupContacts { get; set; }//new
		//public override string GetTableName() => "Contact";
	}
}
