using System.Reflection;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.AddJsonFile(Path.Combine(".", "appsettings.json"));

// Add user secrets from running assembly.
// Note that this is not necessary in ASP.NET Core apps because
// this is done automatically by CreateDefaultBuilder
// (for details see https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows#access-a-secret).
builder.AddUserSecrets(Assembly.GetExecutingAssembly());

var config = builder.Build();
Console.WriteLine($"Foo has value {config["foo"]}.");
Console.WriteLine($"Ping has value {config["ping"]}.");
