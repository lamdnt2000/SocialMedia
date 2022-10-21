using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Reaction
    {
        public long Id { get; set; }
        public int ReactionTypeId { get; set; }
        public long PostId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }

        public virtual PostCrawl Post { get; set; }
        public virtual ReactionType ReactionType { get; set; }
    }
}
