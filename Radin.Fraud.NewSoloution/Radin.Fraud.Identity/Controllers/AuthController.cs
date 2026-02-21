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
		public async Task<ActionResult<string>> Login([FromBody] LoginRequestDTO request)
		{
			var user = await _userManager.FindByNameAsync(request.username);
			if (user == null || !await _userManager.CheckPasswordAsync(user, request.password))
			{
				return Unauthorized("Invalid username or password");
			}

			// Authentication successful, generate JWT
			var token = await _jWTService.GenerateToken(user);
			return Ok(new { Token = token, Result = "Successfully Authorized" });


		}

		public record LoginRequestDTO(string username, string password);
	}
}
