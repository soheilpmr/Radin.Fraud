
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace FraudIdentity.DB.SQL.Data.Entities
{
	public class ApplicationRole : IdentityRole<string>
	{
		[JsonProperty("فعال")]
		public bool IsEnabled { get; set; }
	}
}
