using Microsoft.Data.SqlClient;

try
{
    // Build connection string from scratch
    var builder = new SqlConnectionStringBuilder
    {
        DataSource = "(localdb)\\dev", // Change that to your system's configuration
        InitialCatalog = "master",
        IntegratedSecurity = true,
        TrustServerCertificate = false,
        Encrypt = true,
        ConnectTimeout = 30 // Seconds
    };
    Console.WriteLine(builder.ToString());

    // Parse connection string
    builder = new SqlConnectionStringBuilder("Server=myserver;Integrated Security=true;Database=xyz");

    // Get element from connection string
    Console.WriteLine(builder.InitialCatalog);

    // Change single setting in connection string
    builder.InitialCatalog = "abc";
    Console.WriteLine(builder.ToString());
}
catch (Exception e)
{
    Console.Error.WriteLine($"Error: {e.Message}");
}
