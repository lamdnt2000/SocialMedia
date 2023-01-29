using AutoFilterer.Attributes;
using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel
{
    public class HangfireChannelFilter: PaginationFilterBase
    {
        
        public string Username { get; set; } 
        [ToLowerContainsComparison]
        public string Name { get; set; }
        [Required]
        public int PlatformId { get; set; }
        
    }
}
