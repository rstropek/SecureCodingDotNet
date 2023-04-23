using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using var db = new AddressBookContext();
await WriteToDB(db);
await ReadFromDB(db);

async static Task ReadFromDB(AddressBookContext db)
{
    // Note that the following filter could be a string that a user
    // has entered. We MUST NOT use string concatination to build 
    // WHERE clause in SQL.
    var peopleFilter = "Bar";

    foreach (var person in await db.Persons
        .Where(p => p.LastName.StartsWith(peopleFilter)).ToArrayAsync())
    {
        Console.WriteLine($"{person.LastName}, {person.FirstName}");
    }
}

async static Task WriteToDB(AddressBookContext db)
{
    // Remove all people
    foreach (var p in db.Persons)
    {
        db.Persons.Remove(p);
    }

    // Add two demo rows
    db.Persons.AddRange(new[] {
                new Person() { FirstName = "Tom", LastName = "Turbo" },
                new Person() { FirstName = "Foo", LastName = "Bar" }
            });

    // Save changes to DB
    await db.SaveChangesAsync();
}

class Person
{
    public int ID { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
}

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
    }
}