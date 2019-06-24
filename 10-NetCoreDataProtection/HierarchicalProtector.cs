using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RecapNetCoreConfig
{
    public class HierarchicalProtector
    {
        private readonly Dictionary<string, IDataProtector> protectors;

        public HierarchicalProtector(IDataProtectionProvider provider)
        {
            protectors = new Dictionary<string, IDataProtector> {
                { "Tenant1", provider.CreateProtector("T1") },
                { "Tenant2", provider.CreateProtector("T2") }
            };
        }

        public void Run(string tenant)
        {
            // Create a tenant-specific protector for the purpose
            var protector = protectors[tenant].CreateProtector("API-Key");
            const string stringToProtect = "ABC1234";

            // Protect the payload
            string protectedPayload = protector.Protect(stringToProtect);
            Console.WriteLine($"Ciphertext: {protectedPayload}");

            // Unprotect the payload
            string unprotectedPayload = protector.Unprotect(protectedPayload);
            Console.WriteLine($"Plaintext: {unprotectedPayload}");

            // Try to unprotect with protector of different tenant -> has to fail
            protector = protectors[tenant == "Tenant1" ? "Tenant2" : tenant].CreateProtector("API-Key");
            try
            {
                protector.Unprotect(protectedPayload);
                throw new InvalidOperationException("Expected crypto exception did not happen!");
            }
            catch (CryptographicException)
            {
                Console.WriteLine("Could not unprotect with protector of different tenant.");
            }
        }
    }
}