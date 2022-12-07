using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel.YoutubeStatistic
{
    public class YoutubeStatisticFieldInWeek
    {
        public string DateOfWeek { get; set; }
        public long TotalPost { get; set; }
        public long TotalLike { get; set; }
        public long TotalComment { get; set; }
        public long TotalView { get; set; }
        public double AverageEngagementRateDayOfWeek { get; set; }
        public double AverageEngagementERPostInDayOfWeek { get; set; }
        public double AverageEngagementPostInDayOfWeek { get; set; }
    }
}
