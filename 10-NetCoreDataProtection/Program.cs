using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace RecapNetCoreConfig
{
    public partial class Program
    {
        public static async Task Main(string[] args)
        {
            // Add data protection to service collection. This is
            // typically done in ASP.NET Core apps in ConfigureServices(...).
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection();
            var services = serviceCollection.BuildServiceProvider();

            // Use IDataProtectionProvider in classes. These classes
            // could be controller in ASP.NET Core.
            ActivatorUtilities.CreateInstance<SimpleProtector>(services).Run();
            ActivatorUtilities.CreateInstance<HierarchicalProtector>(services).Run("Tenant1");
            await ActivatorUtilities.CreateInstance<LimitedTimeProtector>(services).Run();
        }
    }
}