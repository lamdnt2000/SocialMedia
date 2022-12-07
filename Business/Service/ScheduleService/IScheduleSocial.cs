using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ScheduleService
{
    public interface IScheduleSocial
    {
        Task FetchChannelJobAsync(string platform, string user, string id);
        Task CreateChannelJobAsync(string platfrom, string user, string id);
        void UpdateChannelJob(string platform, string user, int id);
        void CreateScheduleUpdateChannel(int platformId, string user, int id);
        void TokenSchedule();
        Task SendAsync(string id, string fcm);
        (string, string) ValidateUrl(string url);
        
    }
}
