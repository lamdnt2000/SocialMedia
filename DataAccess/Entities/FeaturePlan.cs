﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("feature_plan")]
    public partial class FeaturePlan
    {
        [Key]
        [Column("plan_id")]
        public int PlanId { get; set; }
        [Key]
        [Column("feature_id")]
        public int FeatureId { get; set; }
        [Column("quota")]
        public int? Quota { get; set; }
        [Column("valid")]
        public bool Valid { get; set; }

        [ForeignKey(nameof(FeatureId))]
        [InverseProperty("FeaturePlans")]
        public virtual Feature Feature { get; set; }
        [ForeignKey(nameof(PlanId))]
        [InverseProperty("FeaturePlans")]
        public virtual Plan Plan { get; set; }
    }
}