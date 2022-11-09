using AutoFilterer.Attributes;
using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.PlatFormModel
{
    public class PlatformPaging: PaginationFilterBase
    {
        [ToLowerContainsComparison]
        public string Name { get; set; }
    }
}
