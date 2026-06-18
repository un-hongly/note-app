using DbUp;
using System.Reflection;

namespace NoteAppApi.Infrastructure.Persistence;

public static class DbUpInitializer
{
    public static void Run(string connectionString)
    {
        var upgrader =
            DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.WriteLine("DB Migration Failed:");
            Console.WriteLine(result.Error);
            throw result.Error;
        }

        Console.WriteLine("Database migrated successfully");
    }
}