using AutoFilterer.Attributes;
using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.LocationModel
{
    public class LocationPaging:PaginationFilterBase
    {
        [ToLowerContainsComparison]
        public string Name { get; set; }
        [ToLowerContainsComparison]
        public string Code { get; set; }
    }
}
