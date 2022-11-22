using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Models.ReactionModel
{
    public class InsertReactionDto
    {
        [Required]
        public string PostId { get; set; }
        [Required]
        public int ReactionTypeId { get; set; }
        [Required]
        public long Count { get; set; }
        [Required]
        public bool Status { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime? UpdateDate { get; set; } = DateTime.Now;

    }
}
