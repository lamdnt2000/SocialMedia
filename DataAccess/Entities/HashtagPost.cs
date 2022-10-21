using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class HashtagPost
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TagId { get; set; }

        public virtual HashtagGroup Tag { get; set; }
    }
}
