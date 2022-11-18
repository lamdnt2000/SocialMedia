using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.PostCrawlModel
{
    public class PostCrawlDto: InsertPostCrawlDto
    {
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime? UpdateDate { get; set; }

        public bool Status { get; set; }
    }
}
