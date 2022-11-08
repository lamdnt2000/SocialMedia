﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities
{
    [Table("post_crawl")]
    public partial class PostCrawl
    {
        public PostCrawl()
        {
            PostHashtags = new HashSet<PostHashtag>();
            Reactions = new HashSet<Reaction>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("title")]
        public string Title { get; set; }
        [Required]
        [Column("pid")]
        [StringLength(20)]
        public string Pid { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Required]
        [Column("post_type")]
        [StringLength(50)]
        public string PostType { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("hashtag_id")]
        public int? HashtagId { get; set; }
        [Required]
        [Column("body")]
        public string Body { get; set; }
        [Column("created_date", TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column("update_date", TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }
        [Column("channel_id")]
        public int ChannelId { get; set; }

        [ForeignKey(nameof(ChannelId))]
        [InverseProperty(nameof(ChannelCrawl.PostCrawls))]
        public virtual ChannelCrawl Channel { get; set; }
        [InverseProperty(nameof(PostHashtag.Post))]
        public virtual ICollection<PostHashtag> PostHashtags { get; set; }
        [InverseProperty(nameof(Reaction.Post))]
        public virtual ICollection<Reaction> Reactions { get; set; }
    }
}