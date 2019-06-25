using System;
using System.IO;
using System.Security.Cryptography;

namespace Signing
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
                    byte[] fileHash;
                    byte[] signedHash;

                    // Compute hash values from streams. This is done because 
                    // digital signatures are usually applied to hash values that
                    // represent larger data.
                    using (var stream = File.OpenRead(Path.Join(".", "SampleText.txt")))
                    {
                        fileHash = shHash.ComputeHash(stream);
                    }

                    using (var rsa = new RSACng())
                    {
                        // Create an object implementing the RSA signing algorithm.
                        // Pass in the RSA object in order to pass on the private key.
                        var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);

                        // Set the hash algorithm
                        rsaFormatter.SetHashAlgorithm(HashAlgorithmName.SHA512.ToString());

                        // Create a signature
                        signedHash = rsaFormatter.CreateSignature(fileHash);
                        // If you want to see the signature, uncomment the following line
                        // Console.WriteLine(Convert.ToBase64String(signedHash));

                        // Use a deformatter to verify signature
                        var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                        rsaDeformatter.SetHashAlgorithm(HashAlgorithmName.SHA512.ToString());
                        if (rsaDeformatter.VerifySignature(fileHash, signedHash))
                        {
                            Console.WriteLine("Signature is valid");
                        }
                        else
                        {
                            Console.WriteLine("Signature is invalid");
                        }
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
