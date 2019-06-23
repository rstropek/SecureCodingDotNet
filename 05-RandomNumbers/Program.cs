using System;
using System.Security.Cryptography;

namespace BCryptDemo
{
    class RandomNumbers
    {
        static void Main()
        {
            try
            {
                // Generate 10 passwords
                for (var i = 0; i < 10; i++)
                {
                    Console.WriteLine(CreatePassword(25));
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error: {e.Message}");
            }
        }

        static string CreatePassword(int length)
        {
            return string.Create(length, length, (builder, len) =>
            {
                const byte firstValidAsciiValue = 33; // !
                const byte lastValidAsciiValue = 126; // ~

                // Number of possible characters in password
                const byte alphabetLength = 126 - 33 + 1;
                const byte maxFairValue = (byte.MaxValue / alphabetLength) * alphabetLength;

                using (var rngCsp = new RNGCryptoServiceProvider())
                {
                    var random = new byte[1];
                    for (var i = 0; i < len; i++)
                    {
                        // We ignore values >= maxFairValue because otherwise the
                        // password characters would not be used equally often.
                        do
                        {
                            // Get random byte
                            rngCsp.GetBytes(random);
                        }
                        while (random[0] >= maxFairValue);

                        builder[i] = (char)((random[0] % (lastValidAsciiValue - firstValidAsciiValue + 1)) + firstValidAsciiValue);
                    }
                }
            });
        }
    }
}
