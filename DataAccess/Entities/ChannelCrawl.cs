using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class ChannelCrawl
    {
        public ChannelCrawl()
        {
            PostCrawls = new HashSet<PostCrawl>();
        }

        public long Id { get; set; }

        public virtual Channel Channel { get; set; }
        public virtual ICollection<PostCrawl> PostCrawls { get; set; }
    }
}
