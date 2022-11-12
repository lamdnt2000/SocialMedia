using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.PostCrawlModel
{
    public class InsertPostCrawlDto
    {
        [Required]
        [MaxLength(20)]
        public string Pid { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [MaxLength(50)]
        public string PostType { get; set; }
        public int? HashtagId { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public int ChannelId { get; set; }
    }
}
