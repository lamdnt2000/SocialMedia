using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.NotificationModel
{
    public class Notification
    {
        public class DataPlayload
        {
            [JsonProperty("message")]
            public string Message { get; set; }
        }
        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";
        [JsonProperty("data")]
        public DataPlayload Data { get; set; }
    }

}
