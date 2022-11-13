﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("user")]
    public partial class User
    {
        public User()
        {
            Wallets = new HashSet<Wallet>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        [StringLength(30)]
        public string Username { get; set; }
        [Required]
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [Column("firstname")]
        [StringLength(50)]
        public string Firstname { get; set; }
        [Required]
        [Column("lastname")]
        [StringLength(50)]
        public string Lastname { get; set; }
        [Column("password")]
        [StringLength(255)]
        public string Password { get; set; }
        [Column("phone")]
        [StringLength(15)]
        public string Phone { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("last_login_at", TypeName = "datetime")]
        public DateTime? LastLoginAt { get; set; }
        [Column("created_date", TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column("update_date", TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("fcm_token")]
        [StringLength(255)]
        public string FcmToken { get; set; }
        [Column("access_token")]
        [StringLength(255)]
        public string AccessToken { get; set; }
        [Required]
        [Column("provider")]
        [StringLength(15)]
        public string Provider { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty("Users")]
        public virtual Role Role { get; set; }
        [InverseProperty(nameof(Wallet.Userr))]
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}