using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
        public double AverageEngagementRate { get; set; }
        [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
        public double AverageEngagementViewER { get; set; }
        [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
        public double AverageEngagementView { get; set; }
    }
}
