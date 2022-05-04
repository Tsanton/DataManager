using System.Collections.Generic;

namespace DataManager.Models
{
    public interface IEntity
    {
        public static abstract string GetSchema();
        public static abstract string GetTargetTable();
        /// <summary>
        /// Required if the entity is to be merged into the target table
        /// </summary>
        public static abstract string GetStagingTable();
        /// <summary>
        /// 
        /// </summary>
        public static abstract List<ColumnNameMapping> GetExclusions();
        /// <summary>
        /// Get the columns that are to be inserted to the DB. Specify if you want DB-column names from context of Property Names
        /// </summary>
        public static abstract List<ColumnNameMapping> GetColumnDefinitions<T>(bool includeExclusions);
        /// <summary>
        /// Get the merge into condition columns.
        /// </summary>
        public static abstract List<string> GetMergeColumns();
    }
}