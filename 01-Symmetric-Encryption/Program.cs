using System.Security.Cryptography;

try
{
    const string plaintext = "Here is some data to encrypt!";

    // Create default implementing of AES. We only use this object to get
    // key and IV.
    //
    // The IV is a non-secret, unique value used as an initial input for
    // certain encryption modes, such as Cipher Block Chaining (CBC) or
    // Counter (CTR) mode. The purpose of the IV is to introduce randomness
    // and prevent the same plaintext from producing the same ciphertext
    // when encrypted with the same key. This helps to protect against pattern
    // analysis and certain types of cryptographic attacks.
    //
    // Note: Anyone that you allow to decrypt your data must possess the
    // same key and IV and use the same algorithm. To communicate a symmetric 
    // key and IV to a remote party, you would usually encrypt the symmetric 
    // key by using asymmetric encryption. Sending the key across an insecure 
    // network without encrypting is unsafe, because anyone who intercepts 
    // the key and IV can then decrypt your data.
    // 
    // Use aes.GenerateKey and aes.GenerateIV to generate multiple keys/IVs
    // if necessary.
    using var aes = Aes.Create();
    
    // Encrypt the string to an array of bytes.
    byte[] ciphertext = await EncryptStringToBytesAsync(plaintext, aes.Key, aes.IV);

    // Experiment: Tamper with some bytes in `ciphertext` and see wrongly
    //             decrypted data in `roundtrip` below.

    // Decrypt the bytes to a string.
    string roundtrip = await DecryptStringFromBytesAsync(ciphertext, aes.Key, aes.IV);

    //Display the original data and the decrypted data.
    Console.WriteLine($"Original:\t{plaintext}");
    Console.WriteLine($"Round Trip:\t{roundtrip}");

}
catch (Exception e)
{
    Console.Error.WriteLine($"Error: {e.Message}");
}

static async Task<byte[]> EncryptStringToBytesAsync(string plainText, byte[] Key, byte[] IV)
{
    byte[] encrypted;
    using (var aes = Aes.Create())
    {
        // Copy given key and IV into AES object
        aes.Key = Key;
        aes.IV = IV;

        // Set block cipher mode (https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.ciphermode).
        // CBC is default.
        // aes.Mode = CipherMode.CBC;

        // Set padding mode (https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.paddingmode).
        // PKCS7 is default.        
        // aes.Padding = PaddingMode.PKCS7;

        // Create encryptor
        ICryptoTransform encryptor = aes.CreateEncryptor();

        // Create memory stream that will receive encrypted bytes
        using var msEncrypt = new MemoryStream();

        // Create crypto stream that does the encryption with the given encryptor
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        
        // Create writer with which we write the plaintext for encryption
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            // Write plaintext that should be encrypted to the crypto stream
            await swEncrypt.WriteAsync(plainText);
        }

        // Get encrypted bytes from memory stream
        encrypted = msEncrypt.ToArray();
    }

    return encrypted;
}

static async Task<string> DecryptStringFromBytesAsync(byte[] cipherText, byte[] Key, byte[] IV)
{
    string plaintext = string.Empty;
    using (var aes = Aes.Create())
    {
        // Copy given key and IV into AES object
        aes.Key = Key;
        aes.IV = IV;

        // Create decryptor
        ICryptoTransform decryptor = aes.CreateDecryptor();

        // Create memory stream that will read encrypted bytes from memory (byte[])
        using var msDecrypt = new MemoryStream(cipherText);

        // Create crypto stream that does the decryption with the given decryptor
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

        // Create reader with which we read the ciphertext for decryption
        using var srDecrypt = new StreamReader(csDecrypt);
        
        // Read decrypted plaintext from crypto stream
        plaintext = await srDecrypt.ReadToEndAsync();

    }

    return plaintext;
}
