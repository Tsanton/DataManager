using System.Collections.Generic;
using System.Linq;
using Dapper;
using DataManager.Adapters;
using DataManager.ConnectionFactories;
using DataManager.Models;

namespace DataManager
{
    public class Manager: IManager
    {
        private readonly IDbConnectionFactory _connection;
        private readonly IAdapter _adapter;

        public Manager(IDbConnectionFactory connection, IAdapter adapter, List<SqlMapper.TypeHandler<object>> handles = null)
        {
            _connection = connection;
            _adapter = adapter;
        }
        
        public T ExecuteScalar<T>(string query)
        {
            using var conn = _connection.CreateConnection();
            return conn.ExecuteScalar<T>(query);
        }

        //Think about moving this to adapter
#warning SQL injection
        public T QueryOne<T>(string filter) where T : IEntity
        {
            var columns = T.GetColumnDefinitions<T>(true).Select(x => $"{x.ColumnName} as {x.PropertyName}").ToList();
            var scope = string.Join(",", columns);
            var query = $"select {scope} from {T.GetSchema()}.{T.GetTargetTable()}";
            if (!string.IsNullOrEmpty(filter)) query = $"{query} {filter}";
            using var conn = _connection.CreateConnection();
            return conn.Query<T>(query).First();
        }
        
        //Think about moving this to adapter
#warning SQL injection
        public List<T> QueryMany<T>(string filter) where T : IEntity
        {
            var columns = T.GetColumnDefinitions<T>(true).Select(x => $"{x.ColumnName} as {x.PropertyName}").ToList();
            var scope = string.Join(",", columns);
            var query = $"select {scope} from {T.GetSchema()}.{T.GetTargetTable()}";
            if (!string.IsNullOrEmpty(filter)) query = $"{query} {filter}";
            using var conn = _connection.CreateConnection();
            return conn.Query<T>(query).ToList();
        }

        public void MergeOne<T>(T entity) where T : IEntity
        {
            MergeMany(new List<T>() {entity});
        }
        
        public void MergeMany<T>(List<T> entities) where T : IEntity
        {
            using var conn = _connection.CreateConnection();
            conn.Execute(_adapter.GetCreateTableStatement<T>(), commandTimeout: 300);
            _adapter.BulkUpload(conn, entities, true);
            conn.Execute(_adapter.GetMergeIntoStatement<T>(), commandTimeout: 300);
            conn.Execute(_adapter.GetDeleteTableStatement<T>(), commandTimeout: 300);
        }
        
        public void InsertMany<T>(List<T> entities) where T: IEntity
        {
            using var conn = _connection.CreateConnection();
            _adapter.BulkUpload(conn, entities, false);
        }

        public void InsertOne<T>(T entity) where T : IEntity
        {
            InsertMany(new List<T>() {entity});
        }
    }
}