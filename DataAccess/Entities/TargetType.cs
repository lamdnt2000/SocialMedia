using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class TargetType
    {
        public TargetType()
        {
            Targets = new HashSet<Target>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Field { get; set; }

        public virtual ICollection<Target> Targets { get; set; }
    }
}
