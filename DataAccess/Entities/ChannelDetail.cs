using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class ChannelDetail
    {
        public long Id { get; set; }
        public long TotalFollow { get; set; }
        public long TotalLike { get; set; }
        public long? TotalShare { get; set; }
        public long TotalComment { get; set; }
        public long? TotalView { get; set; }
        public long TotalPost { get; set; }
        public long ChannelId { get; set; }

        public virtual Channel Channel { get; set; }
    }
}
