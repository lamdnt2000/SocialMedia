using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class PostDetail
    {
        public long Id { get; set; }
        public long? TotalView { get; set; }
        public long TotalLike { get; set; }
        public long TotalComment { get; set; }
        public long? TotalShare { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }
        public long PostId { get; set; }

        public virtual PostCrawl Post { get; set; }
    }
}
