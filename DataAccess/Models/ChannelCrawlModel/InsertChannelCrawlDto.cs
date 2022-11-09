using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel
{
    public class InsertChannelCrawlDto
    {
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int CategoryId { get; set; }
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
        [Required]
        [MaxLength(255)]
        public string AvatarUrl { get; set; }
        [Required]
        [MaxLength(255)]
        public string BannerUrl { get; set; }
        public int Status { get; set; }
        public bool IsVerify { get; set; }
        public string? Bio { get; set; }
        [MaxLength(50)]
        public string? Username { get; set; }
        [Required, MaxLength(255)]
        public string Cid { get; set; }
    }
}
