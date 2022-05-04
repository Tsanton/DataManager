using System.Collections.Generic;
using System.Data;
using DataManager.Models;

namespace DataManager.Adapters
{
    public interface IAdapter
    {
        public void BulkUpload<T>(IDbConnection connection, List<T> entities, bool isMerge) where T : IEntity;

        public string GetCreateTableStatement<T>() where T : IEntity;

        public string GetInsertStatement<T>(bool isMerge) where T : IEntity;

        public string GetMergeIntoStatement<T>() where T : IEntity;

        public string GetDeleteTableStatement<T>() where T : IEntity;
    }
}