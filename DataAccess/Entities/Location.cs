using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Location
    {
        public Location()
        {
            Channels = new HashSet<Channel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Channel> Channels { get; set; }
    }
}
