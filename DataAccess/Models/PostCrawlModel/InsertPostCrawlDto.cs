using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models.ReactionModel;
using System.Text.Json.Serialization;

namespace DataAccess.Models.PostCrawlModel
{
    public class InsertPostCrawlDto
    {
        [Required]
        [MaxLength(50)]
        public string Pid { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public string PostType { get; set; }
        public int? HashtagId { get; set; }
        public string? Body { get; set; }
        [Required]
        public int ChannelId { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public ICollection<InsertReactionDto> Reactions { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}
