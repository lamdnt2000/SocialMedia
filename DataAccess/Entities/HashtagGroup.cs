using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class HashtagGroup
    {
        public HashtagGroup()
        {
            HashtagPosts = new HashSet<HashtagPost>();
        }

        public int Id { get; set; }
        public int HashtagId { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public int MemberId { get; set; }

        public virtual Member Member { get; set; }
        public virtual ICollection<HashtagPost> HashtagPosts { get; set; }
    }
}
