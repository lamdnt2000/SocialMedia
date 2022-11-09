using AutoFilterer.Attributes;
using AutoFilterer.Types;
using DataAccess.Models.PlatFormModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.CategoryModel
{
    public class CategoryPaging: PaginationFilterBase
    {
        public int PlatformId { get; set; }
        [ToLowerContainsComparison]
        public string Name { get; set; }
        //public PlatformPaging Platforms { get; set; } 

    }
}
