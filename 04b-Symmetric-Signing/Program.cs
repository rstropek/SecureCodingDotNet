using System;
using System.IO;
using System.Security.Cryptography;

try
{
    // Create a random key using a random number generator. This is the
    // secret key shared by sender and receiver.
    var secretKey = RandomNumberGenerator.GetBytes(64);

    // Use the secret key to sign the message file.
    SignFile(secretKey, "SampleText.txt", "SignedSampleText.txt");

    // Verify the signed file
    VerifyFile(secretKey, "SignedSampleText.txt");
}
catch (Exception e)
{
    Console.Error.WriteLine($"Error: {e.Message}");
}

static void SignFile(byte[] key, String sourceFile, String destinationFile)
{
    // Initialize the keyed hash object.
    using var hmac = new HMACSHA512(key);

    using var inStream = new FileStream(sourceFile, FileMode.Open);
    using var outStream = new FileStream(destinationFile, FileMode.Create);
    
    // Compute the hash of the input file.
    byte[] hashValue = hmac.ComputeHash(inStream);
    
    // Write the computed hash value to the output file.
    outStream.Write(hashValue, 0, hashValue.Length);

    // Copy the contents of the sourceFile to the destinationFile.
    inStream.Position = 0;
    inStream.CopyTo(outStream);
}

// Compares the key in the source file with a new key created for the data portion of the file. If the keys
// compare the data has not been tampered with.
static bool VerifyFile(byte[] key, String sourceFile)
{
    bool err = false;

    // Initialize the keyed hash object.
    using (var hmac = new HMACSHA512(key))
    {
        // Create an array to hold the keyed hash value read from the file.
        byte[] storedHash = new byte[hmac.HashSize / 8];
        
        // Create a FileStream for the source file.
        using var inStream = new FileStream(sourceFile, FileMode.Open);
        
        // Read in the storedHash.
        inStream.ReadExactly(storedHash);
        
        // Compute the hash of the remaining contents of the file.
        // The stream is properly positioned at the beginning of the content,
        // immediately after the stored hash value.
        byte[] computedHash = hmac.ComputeHash(inStream);

        // compare the computed hash with the stored value
        err = !computedHash.SequenceEqual(storedHash);
    }
    if (err)
    {
        Console.WriteLine("Hash values differ! Signed file has been tampered with!");
        return false;
    }
    else
    {
        Console.WriteLine("Hash values agree -- no tampering occurred.");
        return true;
    }
}