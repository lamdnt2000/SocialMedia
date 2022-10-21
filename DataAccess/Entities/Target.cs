using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Target
    {
        public Target()
        {
            ChannelManagements = new HashSet<ChannelManagement>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double? Progress { get; set; }
        public int TargetTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }
        public long ChannelId { get; set; }

        public virtual TargetType TargetType { get; set; }
        public virtual ICollection<ChannelManagement> ChannelManagements { get; set; }
    }
}
