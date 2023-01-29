using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ScheduleService
{
    public interface IScheduleSocial
    {
        bool FetchChannelJobAsync(string url,string id);
        /*Task FacebookFetchAsync(string user, string id);
        Task YoutubeFetchAsync(string user, string id);
        Task TiktokFetchAsync(string user, string id);
        Task FacebookParserData(string user, int cid, string id);
        Task YoutubeParserData(string user, int cid, string id);
        Task TiktokParserData(string user, int cid, string id);*/
        void UpdateChannelJob(int platformId, string user, int id);
        Task SendNotificationAsync(string key, Notification noti);
        void TokenSchedule();
        (string, string) ValidateUrl(string url);
        
    }
}
