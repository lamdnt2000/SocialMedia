using DataAccess.Entities;
using DataAccess.Models.ChannelRecordModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel
{
    public class ChannelStatisticDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Location { get; set; }
        public string Platform { get; set; }
        public int PlatformId { get; set; }
        public string? Brand { get; set; }
        public string Organization { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string AvatarUrl { get; set; }
        public string BannerUrl { get; set; }
        public int Status { get; set; }
        public bool IsVerify { get; set; }
        public string? Bio { get; set; }
        public string? Username { get; set; }
        public string Cid { get; set; }
        public DateTime CreatedTime { get; set; }
        public ICollection<ChannelRecordDto> ChannelRecords { get; set; }
        public ICollection<string> Categories { get; set; }
        
    }
}
