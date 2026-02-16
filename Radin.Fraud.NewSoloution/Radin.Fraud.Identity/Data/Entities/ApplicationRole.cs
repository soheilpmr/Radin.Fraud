using BackEndInfrsastructure.Domain;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Radin.Fraud.Identity.Data.Entities
{
	public class ApplicationRole : IdentityRole<int>
	{
		[JsonProperty("فعال")]
		public bool IsEnabled { get; set; }
	}
}
