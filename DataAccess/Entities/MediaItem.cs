using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class MediaItem
    {
        public long Id { get; set; }
        public int Size { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int MediaTypeId { get; set; }
        public string Url { get; set; }
        public long PostId { get; set; }

        public virtual MediaType MediaType { get; set; }
        public virtual Post Post { get; set; }
    }
}
