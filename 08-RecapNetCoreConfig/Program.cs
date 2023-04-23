﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// Work with in-memory configuration
// Should print "First Red, then Blue"
var builder = new ConfigurationBuilder();
builder.AddInMemoryCollection(new Dictionary<string, string?>
{
    { "color", "Red" }
});
var config = builder.Build();
Console.Write($"First {config["color"]}, then ");
config["color"] = "Blue";
Console.WriteLine(config["color"]);

// Add JSON configuration
// Note ":" in keys to represent hierarchical structures
builder.AddJsonFile(Path.Combine(".", "appsettings.json"));
config = builder.Build();
Console.WriteLine($"Text is {config["text:color"]}, Circle is {config["circle:color"]}, and {config["color"]} is still there.");

// Add command line
// Try running "dotnet run --color Green"
builder.AddCommandLine(args);
config = builder.Build();
Console.WriteLine($"After adding command line it is {config["color"]}.");

// Add environment variables
// Try running the app after "SET color=Purple"
builder.AddEnvironmentVariables();
config = builder.Build();
Console.WriteLine($"After adding environment variables it is {config["color"]}.");

// Now the "options pattern"
var serviceCollection = new ServiceCollection();
serviceCollection.AddOptions();
serviceCollection.Configure<PaintOptions>(config.GetSection("paintOptions"));
serviceCollection.AddTransient(typeof(ColorPrinter));

// Get options from service provider
var provider = serviceCollection.BuildServiceProvider();
var opt = provider.GetService<IOptions<PaintOptions>>()!;
var oldForeground = Console.ForegroundColor;
var oldBackground = Console.BackgroundColor;
Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), opt.Value.Foreground);
Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), opt.Value.Background);
Console.WriteLine(opt.Value.Background);
Console.ForegroundColor = oldForeground;
Console.BackgroundColor = oldBackground;

// Use options in depdency injection
var painter = provider.GetService<ColorPrinter>();

public class ColorPrinter
{
    public ColorPrinter(IOptions<PaintOptions> options)
    {
        Console.WriteLine($"Got {options.Value.Foreground} and {options.Value.Background}");
    }
}

public class PaintOptions
{
    public string Foreground { get; set; } = "";
    public string Background { get; set; } = "";
}
