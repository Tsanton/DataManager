using System;
using System.Data;
using Dapper;
using Npgsql;
using NpgsqlTypes;

namespace DataManagerTests.Handlers.Postgres;


public class DateOnlyHandle : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
        ((NpgsqlParameter)parameter).NpgsqlDbType = NpgsqlDbType.Date;
    }

    public override DateOnly Parse(object value)
    {
        return DateOnly.Parse((string)value);
    }
}
