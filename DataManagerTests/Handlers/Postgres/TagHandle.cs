using System.Data;
using Dapper;
using DataManagerTests.Entities.Postgres;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Npgsql;
using NpgsqlTypes;

namespace DataManagerTests.Handlers.Postgres;

public class TagHandle : SqlMapper.TypeHandler<Tag>
{
    private static readonly DefaultContractResolver ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };

    public override void SetValue(IDbDataParameter parameter, Tag value)
    {
        parameter.Value = JsonConvert.SerializeObject(value, new JsonSerializerSettings { ContractResolver = ContractResolver });
        ((NpgsqlParameter)parameter).NpgsqlDbType = NpgsqlDbType.Jsonb;
    }

    public override Tag Parse(object value)
    {
        return JsonConvert.DeserializeObject<Tag>(value.ToString(), new JsonSerializerSettings { ContractResolver = ContractResolver });
    }
}