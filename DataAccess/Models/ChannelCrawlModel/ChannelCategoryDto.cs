using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel
{
    public class ChannelCategoryDto
    {
        public int ChannelId { get; set; }
        [Required]
        public long CategoryId { get; set; }
    }
}
