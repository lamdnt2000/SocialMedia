using AutoFilterer.Attributes;
using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel
{
    public class ChannelSearchFilter:  PaginationFilterBase
    {
        public int? OrganizationId { get; set; }
        public int? BrandId { get; set; }
        public int? PlatformId { get; set; }
        [ToLowerContainsComparison]
        public string Name { get; set; }
        public int Status { get; set; } = 1;
    }
}
