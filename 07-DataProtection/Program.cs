using System.Security.Cryptography;

// Create byte array for additional entropy when using Protect method.
byte[] s_additionalEntropy = { 9, 8, 7, 6, 5 };

// Create a simple byte array containing data to be encrypted.
byte[] secret = { 0, 1, 2, 3, 4, 1, 2, 3, 4 };

Console.WriteLine("The secret data is:");
PrintValues(secret);

// Encrypt the data.
byte[] encryptedSecret = Protect(secret)!;
Console.WriteLine("\nThe encrypted byte array is:");
PrintValues(encryptedSecret);

// Decrypt the data and store in a byte array.
byte[] originalData = Unprotect(encryptedSecret)!;
Console.WriteLine("\nThe original data is:");
PrintValues(originalData);

byte[]? Protect(byte[] data)
{
    try
    {
        // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
        // only by the same current user.
        return ProtectedData.Protect(data, s_additionalEntropy, DataProtectionScope.CurrentUser);
    }
    catch (CryptographicException e)
    {
        Console.WriteLine("Data was not encrypted. An error occurred.");
        Console.WriteLine(e.ToString());
        return null;
    }
}

byte[]? Unprotect(byte[] data)
{
    try
    {
        //Decrypt the data using DataProtectionScope.CurrentUser.
        return ProtectedData.Unprotect(data, s_additionalEntropy, DataProtectionScope.CurrentUser);
    }
    catch (CryptographicException e)
    {
        Console.WriteLine("Data was not decrypted. An error occurred.");
        Console.WriteLine(e.ToString());
        return null;
    }
}

static void PrintValues(Byte[] myArr)
{
    foreach (var i in myArr)
    {
        Console.Write($"\t{i}");
    }
    
    Console.WriteLine();
}
