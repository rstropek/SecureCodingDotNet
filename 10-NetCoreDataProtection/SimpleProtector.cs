using Microsoft.AspNetCore.DataProtection;
using System;

namespace RecapNetCoreConfig;

public class SimpleProtector
{
    private readonly IDataProtector protector;

    public SimpleProtector(IDataProtectionProvider provider)
    {
        protector = provider.CreateProtector("Simple");
    }

    public void Run()
    {
        const string stringToProtect = "Secret";

        // Protect the payload
        string protectedPayload = protector.Protect(stringToProtect);
        Console.WriteLine($"Ciphertext: {protectedPayload}");

        // Unprotect the payload
        string unprotectedPayload = protector.Unprotect(protectedPayload);
        Console.WriteLine($"Plaintext: {unprotectedPayload}");
    }
}
