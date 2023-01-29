using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel
{
    public class ChannelFilter: PaginationFilterBase
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public int Platform { get; set; }
        [Required]
        public Range<DateTime> CreatedTime { get; set; }
    }
}
