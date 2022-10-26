﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities
{
    [Table("reaction_type")]
    public partial class ReactionType
    {
        public ReactionType()
        {
            Reactions = new HashSet<Reaction>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }

        [InverseProperty(nameof(Reaction.ReactionType))]
        public virtual ICollection<Reaction> Reactions { get; set; }
    }
}