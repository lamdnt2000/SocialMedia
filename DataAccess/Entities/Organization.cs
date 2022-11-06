﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities
{
    [Table("organization")]
    public partial class Organization
    {
        public Organization()
        {
            Brands = new HashSet<Brand>();
            ChannelCrawls = new HashSet<ChannelCrawl>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }
        [Column("site")]
        [StringLength(255)]
        public string Site { get; set; }

        [InverseProperty(nameof(Brand.Organization))]
        public virtual ICollection<Brand> Brands { get; set; }
        [InverseProperty(nameof(ChannelCrawl.Organization))]
        public virtual ICollection<ChannelCrawl> ChannelCrawls { get; set; }
    }
}