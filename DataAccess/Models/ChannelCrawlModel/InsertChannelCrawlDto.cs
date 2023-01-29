using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Models.ChannelRecordModel;
using DataAccess.Models.PostCrawlModel;
using System.Text.Json.Serialization;

namespace DataAccess.Models.ChannelCrawlModel
{
    public class InsertChannelCrawlDto
    {
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int PlatformId { get; set; }
        public int? BrandId { get; set; }
        [Required]
        public int OrganizationId { get; set; }
        public int? BotId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string Url { get; set; }   
        public string AvatarUrl { get; set; }
        public string BannerUrl { get; set; }
        public int Status { get; set; }
        public bool IsVerify { get; set; }
        public string? Bio { get; set; }
        [MaxLength(50)]
        public string? Username { get; set; }
        [Required, MaxLength(255)]
        public string Cid { get; set; }
        public ICollection<ChannelCategoryDto>? ChannelCategories { get; set; }
        public InsertChannelRecordDto ChannelRecord { get; set; }
        public ICollection<InsertPostCrawlDto> PostCrawls { get; set; }
        public DateTime CreatedTime { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}
