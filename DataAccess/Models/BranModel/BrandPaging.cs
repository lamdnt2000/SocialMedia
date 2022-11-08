using AutoFilterer.Attributes;
using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.BranModel
{
    public class BrandPaging : PaginationFilterBase
    {
        public int OrganizationId { get; set; }
        [ToLowerContainsComparison]
        public string Name { get; set; }
        public bool Status { get; set; } = true;
    }
}
