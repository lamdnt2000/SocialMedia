using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel.TiktokStatistic
{
    public class TiktokStatisticDto : ChannelStatisticDto
    {
        public List<TiktokStatisticField> StatisticFields { get; set; }
        public List<TiktokStatisticFieldInWeek> StatisticFieldsInWeek { get; set; }
        public double AveragePostInDay { get; set; }
        public double AveragePostInWeek { get; set; }
        public double AveragePostInMonth { get; set; }
        public double AverageEngagementLikeInPost { get; set; }
        public double AverageEngagementViewInPost { get; set; }
        public double AverageEngagementCommentRateInPost { get; set; }
        public double AverageEngagementShareInPost { get; set; }
    }
}
