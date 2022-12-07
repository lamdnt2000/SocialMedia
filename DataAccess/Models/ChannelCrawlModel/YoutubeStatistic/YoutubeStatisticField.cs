using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel.YoutubeStatistic
{
    public class YoutubeStatisticField
    {
        public DateTime Date { get; set; }
        public long TotalPost { get; set; }
        public long TotalLike { get; set; }
        public long TotalComment { get; set; }
        public long TotalView { get; set; }
        public double AverageEngagementRate { get; set; }
        public double AverageEngagementViewER { get; set; }
        public double AverageEngagementView { get; set; }
    }
}
