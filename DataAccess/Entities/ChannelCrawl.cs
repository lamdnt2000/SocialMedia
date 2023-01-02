﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("channel_crawl")]
    public partial class ChannelCrawl : BaseEntity
    {
        public ChannelCrawl()
        {
            ChannelCategories = new HashSet<ChannelCategory>();
            ChannelRecords = new HashSet<ChannelRecord>();
            PostCrawls = new HashSet<PostCrawl>();
            Watchlists = new HashSet<Watchlist>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
      
        [Column("location_id")]
        public int LocationId { get; set; }
        [Column("platform_id")]
        public int PlatformId { get; set; }
        [Column("brand_id")]
        public int? BrandId { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("bot_id")]
        public int? BotId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [Column("url")]
        [StringLength(255)]
        public string Url { get; set; }
        [Required]
        [Column("avatar_url")]
        public string AvatarUrl { get; set; }
        [Required]
        [Column("banner_url")]
        public string BannerUrl { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("is_verify")]
        public bool IsVerify { get; set; }
        [Column("bio")]
        public string Bio { get; set; }
        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [Column("cid")]
        [StringLength(255)]
        public string Cid { get; set; }
        [Column("created_time", TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }

        [ForeignKey(nameof(LocationId))]
        [InverseProperty("ChannelCrawls")]
        public virtual Location Location { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("ChannelCrawls")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(PlatformId))]
        [InverseProperty("ChannelCrawls")]
        public virtual Platform Platform { get; set; }
        [InverseProperty(nameof(ChannelCategory.Channel))]
        public virtual ICollection<ChannelCategory> ChannelCategories { get; set; }
        [InverseProperty(nameof(ChannelRecord.Channel))]
        public virtual ICollection<ChannelRecord> ChannelRecords { get; set; }
        [InverseProperty(nameof(PostCrawl.Channel))]
        public virtual ICollection<PostCrawl> PostCrawls { get; set; }
        [InverseProperty(nameof(Watchlist.Channel))]
        public virtual ICollection<Watchlist> Watchlists { get; set; }
    }
}