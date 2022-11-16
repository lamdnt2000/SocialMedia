using DataAccess.Models.ChannelRecordModel;
using DataAccess.Models.LocationModel;
using DataAccess.Models.OrganizationModel;
using DataAccess.Models.PostCrawlModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel
{
    public class ChannelCrawlDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int LocationId { get; set; }
        public int PlatformId { get; set; }
        public int? BrandId { get; set; }
        public int OrganizationId { get; set; }
        public int? BotId { get; set; }
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
        public LocationDto Location { get; set; }
        public OrganizationDto Organization { get; set; }
    }
}
