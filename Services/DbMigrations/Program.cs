using DbUp;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var migrations = new Dictionary<string, string?>
{
    { "Sql/EventDb", configuration.GetConnectionString("EventDb") },
    { "Sql/BasketDb", configuration.GetConnectionString("BasketDB") },
    { "Sql/DiscountDb", configuration.GetConnectionString("DiscountDB") }
};

foreach (var (filePath, connectionString) in migrations)
{
    if (connectionString is null)
    {
        PrintError($"No connection string for filepath: {filePath}");
        continue;
    }

    var rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    var fullPath = Path.Combine(rootPath, filePath);

    if (!Directory.Exists(fullPath))
    {
        Console.WriteLine($"Migration file not found: {fullPath}");
        return -1;
    }

    EnsureDatabase.For.PostgresqlDatabase(connectionString);

    var upgrader =
        DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsFromFileSystem(fullPath)
            .LogToConsole()
            .Build();

    var result = upgrader.PerformUpgrade();

    if (!result.Successful)
    {
        PrintException(result.Error);
#if DEBUG
        Console.ReadLine();
#endif
        return -1;
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Database migration completed successfully!");
    Console.ResetColor();
}

return 0;

void PrintError(string error)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(error);
    Console.ResetColor();
}

void PrintException(Exception error)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(error);
    Console.ResetColor();
}