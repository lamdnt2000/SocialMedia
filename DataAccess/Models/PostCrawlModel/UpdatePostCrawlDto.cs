using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.PostCrawlModel
{
    public class UpdatePostCrawlDto:InsertPostCrawlDto
    {
        public bool Status { get; set; }
    }
}
