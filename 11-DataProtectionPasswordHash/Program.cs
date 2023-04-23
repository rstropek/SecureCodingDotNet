using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

// Note: Consider using BCrypt instead of hashing passwords with
//       ASP.NET Core Data Protection API. BCrypt covers a lot
//       of topics automatically (e.g. choosing salt) that you
//       have to care for yourself when using Data Protection API.

const string secretPassword = "dJJcSb'7A5P@\\$TPhzj`H*M(fSyV%vNvz8(l";

// generate a 128-bit salt using a secure PRNG
var salt = new byte[128 / 8];
using (var rng = RandomNumberGenerator.Create())
{
    rng.GetBytes(salt);
}

// Derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
    password: secretPassword,
    salt: salt,
    prf: KeyDerivationPrf.HMACSHA256,
    iterationCount: 10000,
    numBytesRequested: 256 / 8));
Console.WriteLine($"Hashed: {hashed}");
