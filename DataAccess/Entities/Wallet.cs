using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Wallet
    {
        public Wallet()
        {
            Members = new HashSet<Member>();
            TransactionDeposits = new HashSet<TransactionDeposit>();
        }

        public int Id { get; set; }
        public int Balance { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public int MemberId { get; set; }

        public virtual Member Member { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<TransactionDeposit> TransactionDeposits { get; set; }
    }
}
