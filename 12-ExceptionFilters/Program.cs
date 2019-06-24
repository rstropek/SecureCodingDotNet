using System;

namespace ExceptionFilters
{
    public partial class Program
    {
        public static void Main()
        {
            // This is a local function used for exception filtering
            bool Check(DivideByZeroException _)
            {
                Console.WriteLine("Inside of Check method");
                if (hasElevatedPermissions)
                {
                    // Note that in the exception filter we still have
                    // elevated permissions.
                    Console.WriteLine("I still have elevated permissions in exception filter!");
                }

                return true;
            }

            // Note exception filtering in the following try/catch statement
            try
            {
                DoSomethingWithElevatedPermissions();

                // Uncomment the following line to see how the ..._DoneRight() method
                // fixes the problem (see also https://docs.microsoft.com/en-us/dotnet/framework/misc/securing-exception-handling).
                // DoSomethingWithElevatedPermissions_DoneRight();
            }
            catch (DivideByZeroException ex) when (Check(ex))
            {
                Console.WriteLine("Exception happened");
                if (hasElevatedPermissions)
                {
                    // This will not be called because catch will be called
                    // after finally in DoSomethingWithElevatedPermissions
                    Console.WriteLine("I still have elevated permissions in catch block!");
                }
            }
        }

        static bool hasElevatedPermissions = false;

        public static void DoSomethingWithElevatedPermissions()
        {
            try
            {
                Console.WriteLine("Getting elevated permissions");
                hasElevatedPermissions = true;

                PerformAdministrativeTask();
            }
            finally
            {
                Console.WriteLine("Removing elevated permissions");
                hasElevatedPermissions = false;
            }
        }

        public static void PerformAdministrativeTask()
        {
            if (!hasElevatedPermissions)
            {
                throw new InvalidOperationException("This operation requires elevated permissions");
            }

            // Do something that throws an exception
            var x = 42;
            var y = 0;
            var z = x / y;
        }

        public static void DoSomethingWithElevatedPermissions_DoneRight()
        {
            try
            {
                try
                {
                    Console.WriteLine("Getting elevated permissions");
                    hasElevatedPermissions = true;

                    PerformAdministrativeTask();
                }
                finally
                {
                    Console.WriteLine("Removing elevated permissions");
                    hasElevatedPermissions = false;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}