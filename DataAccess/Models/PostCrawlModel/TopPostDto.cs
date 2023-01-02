using DataAccess.Models.ReactionModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.PostCrawlModel
{
    public class TopPostDto
    {
        public string Pid { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string PostType { get; set; }
        public string? Body { get; set; }
        public int ChannelId { get; set; }
        public bool Status { get; set; }
        public ICollection<InsertReactionDto> Reactions { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
