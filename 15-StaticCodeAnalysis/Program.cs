using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace StaticCodeAnalysis;

[SuppressMessage("", "CA1052", Justification = "Suppress warning for demo")]
public class Program
{
    public static async Task Main()
    {
        var country = Console.ReadLine();

        using (var connection = new SqlConnection("..."))
        {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
            await connection.OpenAsync();
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = $"SELECT 'Foo' AS CustomerName UNION ALL SELECT 'Bar'";

                // Uncomment the following line to see how Roslyn Code Analysis
                // detects potential SQL injection.
                // cmd.CommandText = $"SELECT 'Foo' AS CustomerName WHERE '{country}' = '' OR 'AT' = '{country}' UNION ALL SELECT 'Bar'";

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var result = new List<string>();
                    var customerNameOrdinal = reader.GetOrdinal("CustomerName");
                    while (await reader.ReadAsync())
                    {
                        result.Add(reader.GetString(customerNameOrdinal));
                    }
                }
            }
        }
    }
}
