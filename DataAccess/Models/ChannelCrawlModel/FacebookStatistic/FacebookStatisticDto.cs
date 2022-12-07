using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel.FacebookStatistic
{
    public class FacebookStatisticDto : ChannelStatisticDto
    {
        public List<FacebookStatisticField> StatisticFields { get; set; }
        public List<FacebookStatisticFieldInWeek> StatisticFieldsInWeek { get; set; }
        public List<FacebookStatisticFieldPostType> StatisticFieldsPostType { get; set; }
        public double AveragePostInDay { get; set; }
        public double AveragePostInWeek { get; set; }
        public double AveragePostInMonth { get; set; }
        public double AverageEngagementLikeInPost { get; set; }
        public double AverageEngagementReactionRateInPost { get; set; }
        public double AverageEngagementCommentRateInPost { get; set; }
        public double AverageEngagementShareRateInPost { get; set; }
    }
}
