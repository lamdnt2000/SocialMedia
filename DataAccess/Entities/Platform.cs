using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Platform
    {
        public Platform()
        {
            Categories = new HashSet<Category>();
            Channels = new HashSet<Channel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Channel> Channels { get; set; }
    }
}
