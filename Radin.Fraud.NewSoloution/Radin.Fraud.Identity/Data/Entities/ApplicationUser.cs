using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Radin.Fraud.Identity.Data.Entities
{
	public class ApplicationUser : IdentityUser
	{

		public string AllowedIPs { get; set; }
		public string FirstName { get; set; }

		public string LastName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public bool IsEnabled { get; set; }
		public DateTime? LastLogin { get; set; }
		public long RoleId { get; set; }
		public virtual ApplicationRole Role { get; set; }
	}
}
