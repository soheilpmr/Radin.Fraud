using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using FraudIdentity.DB.SQL.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Radin.Fraud.Identity.Services
{
	public class JWTService : IJWTService
	{
		private readonly IConfiguration _config;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;

		public JWTService(UserManager<ApplicationUser> userManager, IConfiguration config, RoleManager<ApplicationRole> roleManager)
		{
			_userManager = userManager;
			_config = config;
			_roleManager = roleManager;
		}

		public async Task<string> GenerateToken(ApplicationUser user)
		{
			var userRoles = await _userManager.GetRolesAsync(user);
			var userClaims = new List<Claim>();
			var roleClaims = new List<Claim>();
			var claimsFromUser = await _userManager.GetClaimsAsync(user);
			userClaims.AddRange(claimsFromUser.Where(c => c.Type == "Permission"));
			foreach (var roleName in userRoles)
			{
				roleClaims.Add(new Claim(JwtClaimTypes.Role, roleName));

				var role = await _roleManager.FindByNameAsync(roleName);
				if (role != null)
				{
					var claimsForRole = await _roleManager.GetClaimsAsync(role);

					roleClaims.AddRange(claimsForRole.Where(c => c.Type == "Permission"));
				}
			}

			var claims = new List<Claim>
				{
					new Claim(JwtRegisteredClaimNames.Sub, user.Id),
					new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new Claim(ClaimTypes.NameIdentifier, user.Id),
					new Claim(ClaimTypes.GivenName, user.UserName),
					new Claim("AspNet.Identity.SecurityStamp", user.SecurityStamp)
				};

			claims.AddRange(roleClaims);
			claims.AddRange(userClaims);


			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTBearerSettings:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


			var token = new JwtSecurityToken(
				issuer: _config["JWTBearerSettings:Issuer"],
				audience: _config["JWTBearerSettings:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddDays(1),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}


		public async Task<ApplicationUser?> GetUserByID(string userId)
		{
			return await _userManager.FindByIdAsync(userId);
		}
	}
}
