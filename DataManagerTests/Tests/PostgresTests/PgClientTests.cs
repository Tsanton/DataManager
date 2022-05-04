using Dapper;
using DataManagerTests.Fixtures;
using Xunit;

namespace DataManagerTests.Tests.PostgresTests
{
    [Collection("PostgresDatabaseCollection")]
    public class PgClientTests : PostgresInstanceFixture
    {
        public PgClientTests(PostgresSetupFixture fixture) : base(fixture) { }

        [Fact]
        public async Task assert_pg_alive()
        {
            /* Arrange */
            string res = "";

            /* Act */
            await using (var conn = DbFactory.CreateConnection())
            {
                res = await conn.ExecuteScalarAsync<string>("select version()");
            }

            /* Assert */
            Assert.False(string.IsNullOrEmpty(res));
        }
    }
}
