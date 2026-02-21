using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;

public class LegacyPasswordHasher<TUser> : IPasswordHasher<TUser> 
	where TUser : class
{
	// Keep the default hasher for new users and already-upgraded passwords
	private readonly PasswordHasher<TUser> _defaultHasher = new PasswordHasher<TUser>();

	public string HashPassword(TUser user, string password)
	{
		// Always use the new, secure ASP.NET Identity algorithm for new passwords
		return _defaultHasher.HashPassword(user, password);
	}

	public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
	{
		// 1. First, try the default Identity hasher. 
		// This handles new users or users who have already had their passwords upgraded.
		var defaultResult = _defaultHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
		if (defaultResult != PasswordVerificationResult.Failed)
		{
			return defaultResult;
		}

		// 2. If the default fails, check if the password matches the legacy SHA1 Hex format
		string legacyHashAttempt = GenerateSha1Hex(providedPassword);

		// Compare the legacy hash with the one in the database
		if (string.Equals(legacyHashAttempt, hashedPassword, StringComparison.OrdinalIgnoreCase))
		{
			// MAGIC HAPPENS HERE:
			// This tells ASP.NET Identity: "The password is correct, but the hash is old. 
			// Please log the user in AND automatically update the database with a new secure hash."
			return PasswordVerificationResult.SuccessRehashNeeded;
		}

		// 3. If both fail, the password is just wrong
		return PasswordVerificationResult.Failed;
	}

	// Your legacy hashing logic
	private string GenerateSha1Hex(string password)
	{
		using (var sha1 = SHA1.Create())
		{
			byte[] bytes = Encoding.UTF8.GetBytes(password);
			byte[] hashBytes = sha1.ComputeHash(bytes);

			StringBuilder sb = new StringBuilder();
			foreach (byte b in hashBytes)
			{
				sb.Append(b.ToString("x2")); // "x2" creates lowercase hex. Use "X2" if your old system used uppercase.
			}
			return sb.ToString();
		}
	}
}