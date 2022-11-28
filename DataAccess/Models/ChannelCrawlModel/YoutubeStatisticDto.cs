using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel
{
    public class YoutubeStatisticDto:ChannelCrawlDto
    {
        public List<YoutubeStatisticField> StatisticFields { get; set; }
        public List<YoutubeStatisticFieldInWeek> StatisticFieldsInWeek { get; set; }
        public List<YoutubeStatisticFieldPostType> StatisticFieldsPostType { get; set; }
        public double AveragePostInDay { get; set; }
        public double AveragePostInWeek { get; set; }
        public double AveragePostInMonth { get; set; }
        public double AverageEngagementLikeInPost { get; set; }
        public double AverageEngagementViewInPost { get; set; }
        public double AverageEngagementCommentRateInPost { get; set; }
    }
}
