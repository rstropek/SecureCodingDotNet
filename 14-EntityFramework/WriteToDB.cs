using System.Threading.Tasks;

namespace EntityFramework
{
    partial class Program
    {
        async static Task WriteToDB(AddressBookContext db) 
        {
            // Remove all people
            foreach(var p in db.Persons)
            {
                db.Persons.Remove(p);
            }

            // Add two demo rows
            db.Persons.AddRange(new [] {
                new Person() { FirstName = "Tom", LastName = "Turbo" },
                new Person() { FirstName = "Foo", LastName = "Bar" }
            });

            // Save changes to DB
            await db.SaveChangesAsync();
        }
    }
}
