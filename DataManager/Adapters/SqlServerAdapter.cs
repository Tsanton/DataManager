using System.Collections.Generic;
using System.Data;
using DataManager.Models;

namespace DataManager.Adapters;

public class SqlServerAdapter: IAdapter
{
    public void BulkUpload<T>(IDbConnection connection, List<T> entities, bool isMerge) where T : IEntity
    {
        throw new System.NotImplementedException();
    }

    public string GetCreateTableStatement<T>() where T : IEntity
    {
        throw new System.NotImplementedException();
    }

    public string GetInsertStatement<T>(bool isMerge) where T : IEntity
    {
        throw new System.NotImplementedException();
    }

    public string GetMergeIntoStatement<T>() where T : IEntity
    {
        throw new System.NotImplementedException();
    }

    public string GetDeleteTableStatement<T>() where T : IEntity
    {
        throw new System.NotImplementedException();
    }
}