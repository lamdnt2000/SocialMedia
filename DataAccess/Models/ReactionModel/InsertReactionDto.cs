using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ReactionModel
{
    public class InsertReactionDto
    {
        [Required]
        public int CreateDate { get; set; }
        public int UpdateDate { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public int ReactionTypeId { get; set; }
    }
}
