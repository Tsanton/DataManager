using System.Data.Common;

namespace DataManager.ConnectionFactories;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}