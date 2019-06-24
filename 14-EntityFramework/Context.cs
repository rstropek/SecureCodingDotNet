using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Data.SqlClient;

namespace EntityFramework
{
    class AddressBookContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(new SqlConnectionStringBuilder()
            {
                DataSource = "(localdb)\\dev",
                InitialCatalog = "EFCorePeople",
                IntegratedSecurity = true
            }.ToString());
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });
    }
}
