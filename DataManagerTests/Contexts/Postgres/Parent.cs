using System;
using System.Collections.Generic;

namespace DataManagerTests.Contexts.Postgres
{
    public partial class Parent
    {
        public Parent()
        {
            Children = new HashSet<Child>();
        }

        public int ParentId { get; set; }
        public Guid ParentUuid { get; set; }
        public string ParentName { get; set; }
        public int ParentAge { get; set; }
        public double ParentWealth { get; set; }
        public DateTime Inserted { get; set; }
        public DateTime Changed { get; set; }

        public virtual ICollection<Child> Children { get; set; }
    }
}