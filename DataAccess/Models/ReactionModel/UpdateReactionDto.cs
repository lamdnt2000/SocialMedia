﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ReactionModel
{
    public class UpdateReactionDto:InsertReactionDto
    {
        [Required]
        public bool Status { get; set; }
    }
}
