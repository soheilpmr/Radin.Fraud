using BackEndInfrsastructure.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json.Serialization;

namespace Radin.Fraud.Core.Data.Domain
{
	public class Contact : Model<int>
	{
		[JsonPropertyName("نام")]
		public required string FirstName { get; set; }
		[JsonPropertyName("نام خانوادگی")]
		public required string LastName { get; set; }
		[JsonPropertyName("پست الکترونیک")]
		public required string Email { get; set; }
		[JsonPropertyName("شماره موبایل")]
		public required string MobileNumber { get; set; }
	}
}
