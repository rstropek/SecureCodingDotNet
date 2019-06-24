using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KeyVaultManagedIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretsController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public SecretsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return configuration.AsEnumerable()
                .Where(c => c.Key.StartsWith("sqlDb"))
                .Select(c => $"{c.Key}: {c.Value}");
        }
    }
}
