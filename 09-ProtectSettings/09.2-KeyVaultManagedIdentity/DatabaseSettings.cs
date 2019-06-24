namespace KeyVaultManagedIdentity
{
    public class DatabaseSettings
    {
        public string DbServer { get; set; }
        public string DbDatabaseName { get; set; }
        public string DbAdminUserSecretUri { get; set; }
        public string DbAdminUserPasswordSecretUri { get; set; }
    }
}
