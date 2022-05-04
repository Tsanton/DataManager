using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Dapper;
using DataManager.Models;
using Z.BulkOperations;

namespace DataManager.Adapters
{
    public class PostgresAdapter: IAdapter
    {
        public void BulkUpload<T>(IDbConnection connection, List<T> entities, bool isMerge) where T : IEntity
        {
            if (entities.Count < 50)
            {
                //Datetime issues when small bulk inserts: 
                //Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported. Note that it's not possible to mix DateTimes with
                //different Kinds in an array/range. See the Npgsql.EnableLegacyTimestampBehavior AppContext switch to enable legacy behavior.'
                connection.Execute(GetInsertStatement<T>(isMerge), entities);
                return;
            }
            var bulk = new BulkOperation((DbConnection)connection);
            bulk.BatchSize = 1000;
            bulk.DestinationSchemaName = T.GetSchema();
            bulk.DestinationTableName = isMerge ? T.GetStagingTable() : T.GetTargetTable();
            bulk.AllowUpdatePrimaryKeys = true;
            bulk.ColumnMappings.AddRange(T.GetColumnDefinitions<T>(false).Select(x => new ColumnMapping
            {
                IsOptional = false,
                AuditMode = ColumnMappingAuditModeType.Inherit,
                CaseSensitive = ColumnMappingCaseSensitiveType.Inherit,
                DestinationName = x.ColumnName,
                SourceName = x.PropertyName
            }));
            bulk.BulkInsert(entities);
        }
        
        public string GetCreateTableStatement<T>() where T: IEntity
        {
            var columns = T.GetColumnDefinitions<T>(false);
            var insertColumns = string.Join(", ", columns.Select(x => x.ColumnName));
            var query =  $@"
            drop table if exists {T.GetSchema()}.{T.GetStagingTable()};
            create table {T.GetSchema()}.{T.GetStagingTable()} as 
            select {insertColumns} from {T.GetSchema()}.{T.GetTargetTable()} 
            where 1=2";
            return query;
        }

        public string GetInsertStatement<T>(bool isMerge) where T : IEntity
        {
            var columns = T.GetColumnDefinitions<T>(false);
            var inserts = string.Join(", ", columns.Select(x => x.ColumnName));
            var parameters = string.Join(", ", columns.Select(x => $"@{x.PropertyName}").ToList());
            var query =  $"insert into {T.GetSchema()}.{(isMerge ? T.GetStagingTable() : T.GetTargetTable())} ({inserts}) values({parameters})";
            return query;
        }

        public string GetMergeIntoStatement<T>() where T : IEntity
        {
            var columns = T.GetColumnDefinitions<T>(false);
            var mergeColumns = T.GetMergeColumns();
            var inserts = string.Join(",", columns.Select(x => x.ColumnName));
            var deltas = columns.Select(x => x.ColumnName).Except(mergeColumns).ToList();
            var merge = string.Join(", ", mergeColumns);
            var updates = string.Join(", ", deltas.Select(x => $"{x} = excluded.{x}").ToList());
            var tDeltas = string.Join(", ", deltas.Select(x => $"t.{x}").ToList());
            var eDeltas = string.Join(", ", deltas.Select(x => $"excluded.{x}").ToList());
            var query = $@"
            INSERT INTO {T.GetSchema()}.{T.GetTargetTable()} as t ({inserts})
            SELECT {inserts}
            FROM {T.GetSchema()}.{T.GetStagingTable()}
            ON CONFLICT ({merge}) DO UPDATE
            SET {updates}
            WHERE ({tDeltas}) IS DISTINCT FROM ({eDeltas})";
            return query;
        }

        public string GetDeleteTableStatement<T>() where T : IEntity
        {
            var query =  @$"
            truncate table {T.GetSchema()}.{T.GetStagingTable()};
            drop table {T.GetSchema()}.{T.GetStagingTable()};";
            return query;
        }
    }
}
