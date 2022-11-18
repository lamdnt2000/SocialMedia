﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("post_crawl")]
    public partial class PostCrawl:BaseEntity
    {
        public PostCrawl()
        {
            Reactions = new HashSet<Reaction>();
        }

        [Column("title")]
        public string Title { get; set; }
        [Key]
        [Column("pid")]
        [StringLength(100)]
        public string Pid { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Required]
        [Column("post_type")]
        [StringLength(50)]
        public string PostType { get; set; }
        [Column("status")]
        public bool Status { get; set; }
        [Column("hashtag_id")]
        public int? HashtagId { get; set; }
        [Column("body")]
        public string Body { get; set; }
        [Column("channel_id")]
        public int ChannelId { get; set; }
        [Column("created_time", TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }
        [ForeignKey(nameof(ChannelId))]
        [InverseProperty(nameof(ChannelCrawl.PostCrawls))]
        public virtual ChannelCrawl Channel { get; set; }
        [InverseProperty(nameof(Reaction.Post))]
        public virtual ICollection<Reaction> Reactions { get; set; }
    }
}