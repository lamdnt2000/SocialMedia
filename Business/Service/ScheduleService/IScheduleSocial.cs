using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ScheduleService
{
    public interface IScheduleSocial
    {
        void FetchChannelJob(string platform, string user);
        Task CreateChannelJobAsync(string platfrom, string user, string fcmToken);
        void UpdateChannelJob(string platform, string user, int id);
        (string, string) ValidateUrl(string url);
        
    }
}
