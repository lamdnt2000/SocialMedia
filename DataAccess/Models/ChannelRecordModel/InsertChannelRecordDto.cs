﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelRecordModel
{
    public class InsertChannelRecordDto
    {
        [Required]
        public long TotalFollower { get; set; }
        [Required]
        public long TotalLike { get; set; }
   
        public long? TotalShare { get; set; }
        [Required]
        public long TotalComment { get; set; }
        [Required]
        public long TotalPost { get; set; }
        
        public long? TotalView { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}