using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Channel
    {
        public Channel()
        {
            ChannelDetails = new HashSet<ChannelDetail>();
            Ranks = new HashSet<Rank>();
        }

        public long Id { get; set; }
        public string Cid { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Avatar { get; set; }
        public string Banner { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
        public string OrganizationName { get; set; }
        public int MemberId { get; set; }
        public int CriteriaId { get; set; }
        public int LocationId { get; set; }
        public int PlatformId { get; set; }
        public string Nickname { get; set; }

        public virtual Criterion Criteria { get; set; }
        public virtual ChannelManagement Id1 { get; set; }
        public virtual ChannelCrawl IdNavigation { get; set; }
        public virtual Location Location { get; set; }
        public virtual Member Member { get; set; }
        public virtual Platform Platform { get; set; }
        public virtual ICollection<ChannelDetail> ChannelDetails { get; set; }
        public virtual ICollection<Rank> Ranks { get; set; }
    }
}
