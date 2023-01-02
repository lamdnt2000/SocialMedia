using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel.FacebookStatistic
{
    public class FacebookStatisticField: BaseFieldStatistic
    {
        
        public long TotalPost { get; set; }
        public long TotalReaction { get; set; }
        public long TotalComment { get; set; }
        public long TotalShare { get; set; }
        public long TotalReactionCare { get; set; }
        public long TotalReactionLike { get; set; }
        public long TotalReactionLove { get; set; }
        public long TotalReactionWow { get; set; }
        public long TotalReactionHaha { get; set; }
        public long TotalReactionSad { get; set; }
        public long TotalReactionAngry { get; set; }
        public double AverageEngagementRate { get; set; }
        public double AverageEngagementERPost { get; set; }
        public double AverageEngagementInPost { get; set; }
        public double AverageDailyEngagementRate { get; set; }
        public double AverageDailyEngagementReaction { get; set; }



    }
}
