using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Post
    {
        public Post()
        {
            MediaItems = new HashSet<MediaItem>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public int? HashtagId { get; set; }
        public DateTime ScheduleTime { get; set; }
        public int PostTypeId { get; set; }
        public long ChannelId { get; set; }
        public string Pid { get; set; }

        public virtual ChannelManagement Channel { get; set; }
        public virtual Hashtag Hashtag { get; set; }
        public virtual ICollection<MediaItem> MediaItems { get; set; }
    }
}
