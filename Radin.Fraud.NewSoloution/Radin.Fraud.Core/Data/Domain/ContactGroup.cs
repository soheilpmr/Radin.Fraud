using BackEndInfrsastructure.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json.Serialization;

namespace Radin.Fraud.Core.Data.Domain
{
	public class ContactGroup : Model<int>
	{
		[JsonPropertyName("نام")]
		public string Name { get; set; }

		[JsonPropertyName("شرح")]
		public string Description { get; set; }
	}
}
