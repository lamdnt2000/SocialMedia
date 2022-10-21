using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class ReactionType
    {
        public ReactionType()
        {
            Reactions = new HashSet<Reaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Reaction> Reactions { get; set; }
    }
}
