using System.ComponentModel.DataAnnotations.Schema;

namespace DataManagerTests.Entities.Postgres;

public partial class NestedObjectEntity
{
    [Column("nested_id")]
    public int NestedId { get; set; }
    [Column("tag")]
    public Tag Tag { get; set; }
    [Column("tags")]
    public List<Tag> Tags { get; set; }
    public DateTime Inserted { get; set; }
    public DateTime Changed { get; set; }
}