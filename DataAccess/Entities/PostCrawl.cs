using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class PostCrawl
    {
        public PostCrawl()
        {
            Comments = new HashSet<Comment>();
            PostDetails = new HashSet<PostDetail>();
            Reactions = new HashSet<Reaction>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PostType { get; set; }
        public int Status { get; set; }
        public int? HashtagId { get; set; }
        public string Description { get; set; }
        public string Pid { get; set; }
        public long ChannelId { get; set; }

        public virtual ChannelCrawl Channel { get; set; }
        public virtual Hashtag Hashtag { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PostDetail> PostDetails { get; set; }
        public virtual ICollection<Reaction> Reactions { get; set; }
    }
}
