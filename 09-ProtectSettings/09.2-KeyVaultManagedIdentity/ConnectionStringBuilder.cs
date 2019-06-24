using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace KeyVaultManagedIdentity
{
    public class ConnectionStringBuilder
    {
        static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private static string connectionString = string.Empty;
        private DatabaseSettings dbSettings;

        public ConnectionStringBuilder(IOptions<DatabaseSettings> dbSettings)
        {
            this.dbSettings = dbSettings.Value;
        }

        public async Task<string> GetConnectionString()
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                await semaphore.WaitAsync();
                try
                {
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        var keyVaultClient = new KeyVaultClient(
                            new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

                        var dbUser = await keyVaultClient.GetSecretAsync(dbSettings.DbAdminUserSecretUri);
                        var dbPassword = await keyVaultClient.GetSecretAsync(dbSettings.DbAdminUserPasswordSecretUri);

                        var builder = new SqlConnectionStringBuilder
                        {
                            TrustServerCertificate = false,
                            Encrypt = true,
                            InitialCatalog = dbSettings.DbDatabaseName,
                            DataSource = $"tcp:{dbSettings.DbServer}",
                            UserID = dbUser.Value,
                            Password = dbPassword.Value
                        };

                        connectionString = builder.ToString();
                        return connectionString;
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }

            return connectionString;
        }
    }
}
