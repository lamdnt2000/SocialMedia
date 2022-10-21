using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Hashtag
    {
        public Hashtag()
        {
            PostCrawls = new HashSet<PostCrawl>();
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PostCrawl> PostCrawls { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
