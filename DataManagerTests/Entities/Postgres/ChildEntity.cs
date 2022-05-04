using System.ComponentModel.DataAnnotations.Schema;

namespace DataManagerTests.Entities.Postgres;

public partial class ChildEntity
{
    [Column("child_id")]
    public int ChildId { get; set; }
    [Column("child_uuid")]
    public Guid ChildUuid { get; set; }
    [Column("parent_id")]
    public int ParentId { get; set; }
    [Column("child_name")]
    public string ChildName { get; set; }
    [Column("child_age")]
    public int ChildAge { get; set; }
    [Column("child_wealth")]
    public double ChildWealth { get; set; }
    public DateTime Inserted { get; set; }
    public DateTime Changed { get; set; }
}