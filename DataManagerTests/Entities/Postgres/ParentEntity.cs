using System.ComponentModel.DataAnnotations.Schema;

namespace DataManagerTests.Entities.Postgres;

public partial class ParentEntity
{
    [Column("parent_id")]
    public int ParentId { get; set; }
    [Column("parent_uuid")]
    public Guid ParentUuid { get; set; }
    [Column("parent_name")]
    public string ParentName { get; set; }
    [Column("parent_age")]
    public int ParentAge { get; set; }
    [Column("parent_wealth")]
    public double ParentWealth { get; set; }
    public DateTime Inserted { get; set; }
    public DateTime Changed { get; set; }
}