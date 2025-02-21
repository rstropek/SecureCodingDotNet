using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KeyVaultManagedIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ConnectionStringBuilder csb;

        public CustomersController(ConnectionStringBuilder csb)
        {
            this.csb = csb;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get([FromQuery] string country)
        {
            var cs = await csb.GetConnectionString();
            using (var connection = new SqlConnection(cs))
            {
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT 'Foo' AS CustomerName WHERE @Country = '' OR 'AT' = @Country UNION ALL SELECT 'Bar'";
                    cmd.Parameters.AddWithValue("@Country", string.IsNullOrEmpty(country) ? string.Empty : country);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var result = new List<string>();
                        var customerNameOrdinal = reader.GetOrdinal("CustomerName");
                        while (await reader.ReadAsync())
                        {
                            result.Add(reader.GetString(customerNameOrdinal));
                        }

                        return result;
                    }
                }
            }
        }
    }
}
