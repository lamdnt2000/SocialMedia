﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    [Table("transaction_deposit")]
    public partial class TransactionDeposit
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("card_type")]
        [StringLength(10)]
        public string CardType { get; set; }
        [Column("amount")]
        public int Amount { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Required]
        [Column("code")]
        [StringLength(20)]
        public string Code { get; set; }
        [Required]
        [Column("trans_no_id")]
        [StringLength(20)]
        public string TransNoId { get; set; }
        [Column("pay_date", TypeName = "datetime")]
        public DateTime PayDate { get; set; }
        [Required]
        [StringLength(5)]
        public string Locale { get; set; }
        [Required]
        [Column("order_infor")]
        [StringLength(50)]
        public string OrderInfor { get; set; }
        [Required]
        [Column("order_type")]
        [StringLength(10)]
        public string OrderType { get; set; }
        [Required]
        [Column("txn_ref")]
        [StringLength(10)]
        public string TxnRef { get; set; }
        [Column("wallet_id")]
        public int WalletId { get; set; }
        [Column("gateway_id")]
        public int GatewayId { get; set; }
        [Required]
        [Column("tmn_code")]
        [StringLength(10)]
        public string TmnCode { get; set; }
        [Column("current_blance")]
        public int CurrentBlance { get; set; }
        [Column("new_balance")]
        public int NewBalance { get; set; }

        [ForeignKey(nameof(GatewayId))]
        [InverseProperty("TransactionDeposits")]
        public virtual Gateway Gateway { get; set; }
        [ForeignKey(nameof(WalletId))]
        [InverseProperty("TransactionDeposits")]
        public virtual Wallet Wallet { get; set; }
    }
}