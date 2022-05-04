using DataManagerTests.ConnectionFactories;
using Respawn;

namespace DataManagerTests.Fixtures;

public class PostgresInstanceFixture
{
    public readonly PostgresConnectionFactory DbFactory;

    private static readonly Checkpoint Checkpoint = new()
    {
        WithReseed = true,
        DbAdapter = Respawn.DbAdapter.Postgres
    };

    
    public PostgresInstanceFixture(PostgresSetupFixture fixture)
    {
        DbFactory = fixture.DbFactory;
        if (!fixture.ConnStr.Contains("test")) throw new Exception("About to nuke non test DB");
        Console.WriteLine("Resetting PG DB to checkpoint");
        Checkpoint.Reset(fixture.DbFactory.CreateConnection()).Wait();
    }
}