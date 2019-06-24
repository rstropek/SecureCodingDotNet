using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace EntityFramework
{
    partial class Program
    {
        async static Task ReadFromDB(AddressBookContext db) 
        {
            // Note that the following filter could be a string that a user
            // has entered. We MUST NOT use string concatination to build 
            // WHERE clause in SQL.
            var peopleFilter = "Bar";

            foreach(var person in await db.Persons
                .Where(p => p.LastName.StartsWith(peopleFilter)).ToArrayAsync())
            {
                Console.WriteLine($"{person.LastName}, {person.FirstName}");
            }
        }
    }
}
