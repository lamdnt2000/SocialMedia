using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelCrawlModel.TiktokStatistic
{
    public class TiktokStatisticDto : ChannelStatisticDto
    {
        public List<TiktokStatisticField> StatisticFields { get; set; }
        public List<TiktokStatisticFieldInWeek> StatisticFieldsInWeek { get; set; }
        [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
        public double AveragePostInDay { get; set; }
        [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
        public double AveragePostInWeek { get; set; }
        [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
        public double AveragePostInMonth { get; set; }
        [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
        public double AverageEngagementLikeInPost { get; set; }
        [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
        public double AverageEngagementViewInPost { get; set; }
        [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
        public double AverageEngagementCommentRateInPost { get; set; }
        [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
        public double AverageEngagementShareInPost { get; set; }
    }
}
