using System;

namespace BCryptDemo
{
    class Program
    {
        static void Main()
        {
            try
            {
                const string secretPassword = "dJJcSb'7A5P@\\$TPhzj`H*M(fSyV%vNvz8(l";

                // Note that this demo lets BCrypt choose the salt. This is the recommended
                // way of hashing passwords with BCrypt

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(secretPassword);
                Console.WriteLine($"The hashed password is {hashedPassword}");

                var hashedPasswordAgain = BCrypt.Net.BCrypt.HashPassword(secretPassword);
                Console.WriteLine($"Hash it again and get  {hashedPasswordAgain}");

                if (BCrypt.Net.BCrypt.Verify(secretPassword, hashedPassword))
                {
                    Console.WriteLine("Everything is fine, password is ok");
                }
                else
                {
                    Console.WriteLine("Attention, password and hash do not match");
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
