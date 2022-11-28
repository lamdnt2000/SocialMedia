using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel
{
    public class FacebookStatisticFieldInWeek
    {
        public DayOfWeek DateOfWeek { get; set; }
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
        public double AverageEngagementERPostInDayOfWeek { get; set; }
        public double AverageEngagementRateDayOfWeek { get; set; }
        public double AverageEngagementPostInDayOfWeek { get; set; }
        
    }
}
