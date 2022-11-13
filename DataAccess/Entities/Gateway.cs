﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("gateway")]
    public partial class Gateway
    {
        public Gateway()
        {
            TransactionDeposits = new HashSet<TransactionDeposit>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Column("access_key")]
        [StringLength(50)]
        public string AccessKey { get; set; }
        [Required]
        [Column("secret_key")]
        [StringLength(50)]
        public string SecretKey { get; set; }
        [Required]
        [Column("type")]
        [StringLength(10)]
        public string Type { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Required]
        [Column("bank_code")]
        [StringLength(50)]
        public string BankCode { get; set; }
        [Required]
        [Column("bank_trans_no")]
        [StringLength(50)]
        public string BankTransNo { get; set; }
        [Column("admin_id")]
        public int AdminId { get; set; }

        [InverseProperty(nameof(TransactionDeposit.Gateway))]
        public virtual ICollection<TransactionDeposit> TransactionDeposits { get; set; }
    }
}