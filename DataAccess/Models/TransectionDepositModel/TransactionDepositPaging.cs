using AutoFilterer.Attributes;
using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.TransectionDepositModel
{
    public class TransactionDepositPaging : PaginationFilterBase
    {
        [ToLowerContainsComparison]
        public int GatewayId { get; set; }
        public int WalletId { get; set; }
    }
}
