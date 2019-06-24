using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework
{
    partial class Program
    {
        static async Task Main(string[] args)
        {
            using (var db = new AddressBookContext())
            {
                await WriteToDB(db);
                await ReadFromDB(db);
            }
        }
    }
}
