using System;

namespace BCryptDemo
{
    class Program
    {
        static void Main()
        {
            try
            {
                var rand = new Random();
                var counter = new int[10];
                for (var i = 0; i < 1000000; i++)
                {
                    counter[rand.Next(counter.Length)]++;
                }

                for (var i = 0; i < counter.Length; i++)
                {
                    Console.WriteLine($"{i}: {counter[i]/1000000d, 7:p3}");
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
