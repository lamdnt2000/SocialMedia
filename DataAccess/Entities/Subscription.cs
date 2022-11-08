﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities
{
    [Keyless]
    [Table("subscription")]
    public partial class Subscription
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("price")]
        public double Price { get; set; }
        [Column("create_date", TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Column("update_date", TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }
        [Column("wallet_id")]
        public int WalletId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("offer_id")]
        public int OfferId { get; set; }
        [Column("package_id")]
        public int PackageId { get; set; }
        [Column("start_date", TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column("end_date", TypeName = "datetime")]
        public DateTime EndDate { get; set; }

        [ForeignKey(nameof(OfferId))]
        public virtual Offer Offer { get; set; }
        [ForeignKey(nameof(PackageId))]
        public virtual Package Package { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        [ForeignKey(nameof(WalletId))]
        public virtual Wallet Wallet { get; set; }
    }
}