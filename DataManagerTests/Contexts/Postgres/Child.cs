using System;

namespace DataManagerTests.Contexts.Postgres
{
    public partial class Child
    {
        public int ChildId { get; set; }
        public Guid ChildUuid { get; set; }
        public int ParentId { get; set; }
        public string ChildName { get; set; }
        public int ChildAge { get; set; }
        public double ChildWealth { get; set; }
        public DateTime Inserted { get; set; }
        public DateTime Changed { get; set; }

        public virtual Parent Parent { get; set; }
    }
}
