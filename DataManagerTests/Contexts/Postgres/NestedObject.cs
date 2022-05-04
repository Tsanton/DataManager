using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DataManagerTests.Entities;
using DataManagerTests.Entities.Postgres;

namespace DataManagerTests.Contexts.Postgres
{
    public partial class NestedObject
    {
        public int NestedId { get; set; }
        public Tag Tag { get; set; }
        public List<Tag> Tags { get; set; }
        public DateTime Inserted { get; set; }
        public DateTime Changed { get; set; }
    }
}
