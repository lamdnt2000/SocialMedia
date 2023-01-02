using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace DataAccess.Models.ChannelRecordModel
{
    public class InsertChannelRecordDto
    {
        [Required]
        public long TotalFollower { get; set; }
        [Required]
        public long TotalLike { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? TotalShare { get; set; }
        [Required]
        public long TotalComment { get; set; }
        [Required]
        public long TotalPost { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? TotalView { get; set; }

        [Required]
        public bool Status { get; set; }

        public int ChannelId { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}
