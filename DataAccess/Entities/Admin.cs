using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Admin
    {
        public Admin()
        {
            Gateways = new HashSet<Gateway>();
        }

        public int Id { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Gateway> Gateways { get; set; }
    }
}
