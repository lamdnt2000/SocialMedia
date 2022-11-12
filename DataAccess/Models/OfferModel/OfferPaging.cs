using AutoFilterer.Attributes;
using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.OfferModel
{
    public class OfferPaging : PaginationFilterBase
    {
        [ToLowerContainsComparison]
        public string Name { get; set; }
    }
}
