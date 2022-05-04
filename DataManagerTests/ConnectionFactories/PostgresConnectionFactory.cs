using System.Collections.Generic;
using System.Data.Common;
using Dapper;
using DataManager.ConnectionFactories;
using Npgsql;

namespace DataManagerTests.ConnectionFactories;

public class PostgresConnectionFactory: IDbConnectionFactory
{
    private readonly string _connectionString;
    
    public PostgresConnectionFactory(string connStr, List<SqlMapper.TypeHandler<object>> handlers = null)
    {
        _connectionString = connStr;
        if (handlers is null) return;
        foreach (var handler in handlers)
        {
            SqlMapper.AddTypeHandler(handler);
        }

    }
    
    public DbConnection CreateConnection()
    {
        var conn = new NpgsqlConnection(_connectionString);
        conn.Open();
        return conn;
    }
}