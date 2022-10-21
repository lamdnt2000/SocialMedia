using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class MediaType
    {
        public MediaType()
        {
            MediaItems = new HashSet<MediaItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MediaItem> MediaItems { get; set; }
    }
}
