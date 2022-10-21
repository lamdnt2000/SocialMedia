using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class RankType
    {
        public RankType()
        {
            Ranks = new HashSet<Rank>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Rank> Ranks { get; set; }
    }
}
