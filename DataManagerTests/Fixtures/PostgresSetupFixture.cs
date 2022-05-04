using DataManagerTests.ConnectionFactories;
using DataManagerTests.Contexts.Postgres;
using Xunit;


namespace DataManagerTests.Fixtures;

public class PostgresSetupFixture
{
    public readonly PostgresConnectionFactory DbFactory;
    public readonly string ConnStr;
    public PostgresSetupFixture()
    {
        var host = Environment.GetEnvironmentVariable("PG_HOST") ?? throw new NotImplementedException();
        var db = Environment.GetEnvironmentVariable("PG_DB") ?? throw new NotImplementedException();
        var uid = Environment.GetEnvironmentVariable("PG_UID") ?? throw new NotImplementedException();
        var pwd = Environment.GetEnvironmentVariable("PG_PWD") ?? throw new NotImplementedException();
        ConnStr = $"Host={host};Database={db};Username={uid};Password={pwd}";
        if (!ConnStr.Contains("test")) throw new Exception("About to nuke non test DB");
        DbFactory = new PostgresConnectionFactory(ConnStr);
        var context = new PostgresContext(ConnStr);
        Console.WriteLine("Migrating PG DB");
        context.Database.EnsureDeleted();
        Console.WriteLine("Deleted PG DB");
        context.Database.EnsureCreated();
        Console.WriteLine("Migrated PG DB");
    }
}

[CollectionDefinition("PostgresDatabaseCollection")]
public class DatabaseCollection : ICollectionFixture<PostgresSetupFixture>
{
    // This class has no code, and is never created. Its purpose is simplyz
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}