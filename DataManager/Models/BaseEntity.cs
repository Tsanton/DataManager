using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataManager.Models;

public static class BaseEntity
{
    public static List<ColumnNameMapping> GetColumnDefinitions<T>(bool includeExclusions, List<ColumnNameMapping> exclusions = null)
    {
        var properties = typeof(T).GetProperties().Select(x => new ColumnNameMapping
        {
            PropertyName = x.Name,
            ColumnName = x.GetCustomAttribute<ColumnAttribute>()?.Name ?? x.Name
        }).ToList();
        if (includeExclusions) return properties;
        return exclusions is null ? properties : properties.Where(x => !exclusions.Select(y => y.PropertyName).Contains(x.PropertyName)).ToList();
    }
}