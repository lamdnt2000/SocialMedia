using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class ChannelManagement
    {
        public ChannelManagement()
        {
            Posts = new HashSet<Post>();
        }

        public long Id { get; set; }
        public int TargetId { get; set; }
        public long TotalFollow { get; set; }
        public long TotalLike { get; set; }
        public long? TotalShare { get; set; }
        public long TotalComment { get; set; }
        public long TotalPost { get; set; }
        public long? TotalView { get; set; }

        public virtual Target Target { get; set; }
        public virtual Channel Channel { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
