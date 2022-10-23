﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    [Table("ChannelManagement")]
    public partial class ChannelManagement
    {
        public ChannelManagement()
        {
            Posts = new HashSet<Post>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("target_id")]
        public int TargetId { get; set; }
        [Column("total_follow")]
        public long TotalFollow { get; set; }
        [Column("total_like")]
        public long TotalLike { get; set; }
        [Column("total_share")]
        public long? TotalShare { get; set; }
        [Column("total_comment")]
        public long TotalComment { get; set; }
        [Column("total_post")]
        public long TotalPost { get; set; }
        [Column("total_view")]
        public long? TotalView { get; set; }

        [ForeignKey(nameof(TargetId))]
        [InverseProperty("ChannelManagements")]
        public virtual Target Target { get; set; }
        [InverseProperty("Id1")]
        public virtual Channel Channel { get; set; }
        [InverseProperty(nameof(Post.Channel))]
        public virtual ICollection<Post> Posts { get; set; }
    }
}