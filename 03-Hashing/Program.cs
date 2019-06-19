using System;
using System.IO;
using System.Security.Cryptography;

namespace Hashing
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Create object implementing SHA512 hash algorithm
                using (var shHash = new SHA512Managed())
                {
                    ReadOnlySpan<byte> originalFileHash, copiedFileHash;

                    // Compute hash values from streams
                    using (var stream = File.OpenRead(Path.Join(".", "SampleText.txt")))
                    {
                        originalFileHash = shHash.ComputeHash(stream);
                    }

                    using (var stream = File.OpenRead(Path.Join(".", "CopiedText.txt")))
                    {
                        copiedFileHash = shHash.ComputeHash(stream);
                    }

                    // Compare hash values
                    if (originalFileHash.SequenceEqual(copiedFileHash))
                    {
                        Console.WriteLine("Files are equal");
                    }
                    else
                    {
                        Console.WriteLine("Files are different");
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
