using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Criterion
    {
        public Criterion()
        {
            Channels = new HashSet<Channel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Channel> Channels { get; set; }
    }
}
