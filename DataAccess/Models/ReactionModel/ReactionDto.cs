using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ReactionModel
{
    public class ReactionDto:InsertReactionDto
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Status { get; set; }
    }
}
