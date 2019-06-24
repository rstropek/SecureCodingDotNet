using Microsoft.AspNetCore.DataProtection;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RecapNetCoreConfig
{
    public class LimitedTimeProtector
    {
        private readonly ITimeLimitedDataProtector protector;

        public LimitedTimeProtector(IDataProtectionProvider provider)
        {
            // Note the conversion to a time-limited protector
            protector = provider.CreateProtector("LimitedTime")
                .ToTimeLimitedDataProtector();
        }

        public async Task Run()
        {
            const string stringToProtect = "AccessToken";

            // Note how expiration time is specified in call to Protect
            string protectedPayload = protector.Protect(stringToProtect, TimeSpan.FromSeconds(1));
            Console.WriteLine($"Ciphertext: {protectedPayload}");

            string unprotectedPayload = protector.Unprotect(protectedPayload);
            Console.WriteLine($"Plaintext: {unprotectedPayload}");

            // Try unprotecting after expiration time -> has to fail
            await Task.Delay(TimeSpan.FromSeconds(2));
            try
            {
                protector.Unprotect(protectedPayload);
                throw new InvalidOperationException("Expected crypto exception did not happen!");
            }
            catch (CryptographicException)
            {
                Console.WriteLine("Could not unprotect after protection expired.");
            }
        }
    }
}