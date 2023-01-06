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
        //public Range<DateTime> CreatedDate { get; set; }
       
    }
}
