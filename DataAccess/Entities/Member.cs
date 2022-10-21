using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Member
    {
        public Member()
        {
            Channels = new HashSet<Channel>();
            HashtagGroups = new HashSet<HashtagGroup>();
            Orders = new HashSet<Order>();
            Wallets = new HashSet<Wallet>();
        }

        public int Id { get; set; }
        public int WalletId { get; set; }
        public string FcmToken { get; set; }

        public virtual Wallet Wallet { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Channel> Channels { get; set; }
        public virtual ICollection<HashtagGroup> HashtagGroups { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
