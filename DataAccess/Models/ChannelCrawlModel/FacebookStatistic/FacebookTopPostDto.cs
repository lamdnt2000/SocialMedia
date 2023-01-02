using DataAccess.Models.PostCrawlModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel.FacebookStatistic
{
    public class FacebookTopPostDto: ChannelStatisticDto
    {
        public ICollection<PostCrawlDto> Posts { get; set; }
    }
}
