using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.NotificationModel
{
    public class PayLoad
    {
        public class DataPlayload
        {
            [JsonProperty("body")]
            public string Body { get; set; }
            [JsonProperty("title")]
            public string Title { get; set; }
        }
        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";
        public bool ContentAvailable { get; set; } = true;
        [JsonProperty("data")]
        public DataPlayload Notification { get; set; }
    }

}
