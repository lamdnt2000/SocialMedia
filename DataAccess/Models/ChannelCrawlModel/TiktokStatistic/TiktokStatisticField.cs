using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel.TiktokStatistic
{
    public class TiktokStatisticField: BaseFieldStatistic
    {
        public long TotalPost { get; set; }
        public long TotalLike { get; set; }
        public long TotalComment { get; set; }
        public long TotalView { get; set; }
        public long TotalShare { get; set; }
        public double AverageEngagementRate { get; set; }
        public double AverageEngagementViewER { get; set; }
        public double AverageEngagementView { get; set; }
    }
}
