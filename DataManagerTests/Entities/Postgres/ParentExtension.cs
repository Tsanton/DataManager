using DataManager.Models;

namespace DataManagerTests.Entities.Postgres;

public partial class ParentEntity : IEntity
{
    public static string GetSchema() => "public";

    public static string GetTargetTable() => "parents";

    public static string GetStagingTable() => $"{GetTargetTable()}_staging";
    
    public static List<ColumnNameMapping> GetExclusions()
    {
        return new List<ColumnNameMapping>
        {
            new ColumnNameMapping {PropertyName = "ParentUuid", ColumnName = "parent_uuid"},
            new ColumnNameMapping{PropertyName = "Inserted", ColumnName = "inserted"},
            new ColumnNameMapping {PropertyName = "Changed", ColumnName = "changed"}
        };
    }
    
    public static List<ColumnNameMapping> GetColumnDefinitions<T>(bool includeExclusions)
    {
        return BaseEntity.GetColumnDefinitions<T>(includeExclusions, GetExclusions());
    }


    public static List<string> GetMergeColumns()
    {
        return new List<string>() { "parent_id" };
    }
}