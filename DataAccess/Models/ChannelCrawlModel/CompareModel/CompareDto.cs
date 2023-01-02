using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel.CompareModel
{
    public class CompareDto
    {
        [Required]
        public string UserIdOne { get; set; }
        [Required]
        public string UserIdTwo { get; set; }
        [Required]
        public int Platform { get; set; }
        [Required]
        public Range<DateTime> CreatedTime { get; set; }
    }
}
