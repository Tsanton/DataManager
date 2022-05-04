using System.Collections.Generic;
using System.Data;
using Dapper;
using DataManagerTests.Entities;
using DataManagerTests.Entities.Postgres;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Npgsql;
using NpgsqlTypes;

namespace DataManagerTests.Handlers.Postgres;

public class TagsHandle : SqlMapper.TypeHandler<List<Tag>>
{
    private static readonly DefaultContractResolver ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };

    public override void SetValue(IDbDataParameter parameter, List<Tag> value)
    {
        parameter.Value = JsonConvert.SerializeObject(value, new JsonSerializerSettings { ContractResolver = ContractResolver });
        ((NpgsqlParameter)parameter).NpgsqlDbType = NpgsqlDbType.Jsonb;
    }

    public override List<Tag> Parse(object value)
    {
        return JsonConvert.DeserializeObject<List<Tag>>(value.ToString(), new JsonSerializerSettings { ContractResolver = ContractResolver });
    }
}