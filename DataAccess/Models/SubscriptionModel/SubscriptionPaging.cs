using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.SubscriptionModel
{
    public class SubscriptionPaging : PaginationFilterBase
    {
        public int WalletId { get; set; }
        public int PackageId { get; set; }
        public int OfferId { get; set; }
        public int UserId { get; set; }
        public Range<DateTime> Offset { get; set; }
        public int Status { get; set; }
    }
}
