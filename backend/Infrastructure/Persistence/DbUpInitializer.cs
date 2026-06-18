using DbUp;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace NoteAppApi.Infrastructure.Persistence;

public static class DbUpInitializer
{
    public static void Run(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var databaseName = builder.InitialCatalog;

        if (!string.IsNullOrEmpty(databaseName))
        {
            builder.InitialCatalog = "master";
            using var masterConnection = new SqlConnection(builder.ConnectionString);
            masterConnection.Open();

            var checkDbCmd = masterConnection.CreateCommand();
            checkDbCmd.CommandText = $@"
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = @dbName)
                BEGIN
                    CREATE DATABASE [{databaseName}]
                END";
            checkDbCmd.Parameters.AddWithValue("@dbName", databaseName);
            checkDbCmd.ExecuteNonQuery();
        }

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