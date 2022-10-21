using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Comment
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorUsername { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public long? ReplyId { get; set; }
        public long CommentId { get; set; }

        public virtual PostCrawl Post { get; set; }
    }
}
