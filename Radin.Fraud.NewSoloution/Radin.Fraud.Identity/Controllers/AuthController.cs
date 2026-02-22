using FraudIdentity.DB.SQL.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radin.Fraud.Identity.Services;

namespace Radin.Fraud.Identity.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IJWTService _jWTService;
		private readonly UserManager<ApplicationUser> _userManager;
		public AuthController(IJWTService jWTService, UserManager<ApplicationUser> userManager)
		{
			_jWTService = jWTService;
			_userManager = userManager;
		}

		[HttpPost(nameof(Login))]
		public async Task<ActionResult> Login([FromBody] LoginRequestDTO request)
		{
			// 1. Find User & Validate Credentials
			// NOTE: Because you registered the LegacyPasswordHasher from earlier, 
			// CheckPasswordAsync will automatically handle your SHA1 Hex logic!
			var user = await _userManager.FindByNameAsync(request.username);

			if (user == null || !await _userManager.CheckPasswordAsync(user, request.password))
			{
				return Unauthorized(new { Message = "نام کاربری یا رمز عبور اشتباه است" });
			}

			// 2. Check if the Account is Enabled
			if (!user.IsEnabled)
			{
				// 403 Forbidden is the standard HTTP status for disabled accounts
				return StatusCode(StatusCodes.Status403Forbidden, new { Message = "حساب کاربری موردنظر غیرفعال است" });
			}

			// 3. IP Restriction Check
			if (!string.IsNullOrEmpty(user.AllowedIPs))
			{
				var ips = user.AllowedIPs.Split('#');
				var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();

				if (remoteIp == null || !ips.Contains(remoteIp))
				{
					return StatusCode(StatusCodes.Status403Forbidden, new { Message = "شما مجاز به ورود به سامانه نیستید" });
				}
			}

			// 4. Cache Permissions (Keeping your existing logic intact)
			// You will need to fetch the roles/permissions from your DB or via _userManager
			// UserPermissionCache.AddUserPermissions(user.UserName, ...);

			// 5. Update Last Login Timestamp
			user.LastLogin = DateTime.Now;
			await _userManager.UpdateAsync(user);

			// 6. Handle Dashboards
			// In APIs, you typically return routing data in the JSON response 
			// rather than doing server-side redirects.
			// var defaultDashboard = await _dashboardRepository.GetDefaultAsync(user.Id);

			// 7. Generate JWT Token
			var token = await _jWTService.GenerateToken(user);

			// 8. Return the token and any UI-routing metadata
			return Ok(new
			{
				Token = token,
				Message = "Successfully Authorized"
				// DashboardId = defaultDashboard?.Id 
			});
		}

		public record LoginRequestDTO(string username, string password);
	}
}
