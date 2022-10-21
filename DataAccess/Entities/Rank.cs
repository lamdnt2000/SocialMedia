using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Rank
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int RankTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public double? Grade { get; set; }
        public long ChannelId { get; set; }

        public virtual Channel Channel { get; set; }
        public virtual RankType RankType { get; set; }
    }
}
