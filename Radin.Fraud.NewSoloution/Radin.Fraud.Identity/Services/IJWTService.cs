

using FraudIdentity.DB.SQL.Data.Entities;

namespace Radin.Fraud.Identity.Services
{
	public interface IJWTService
	{
		Task<string> GenerateToken(ApplicationUser user);
		Task<ApplicationUser?> GetUserByID(string userId);
	}
}
