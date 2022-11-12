using AutoFilterer.Attributes;
using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.WalletModel
{
    public class WalletPaging : PaginationFilterBase
    {
        [ToLowerContainsComparison]
        public string Currency { get; set; }
    }
}
