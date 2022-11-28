using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Schedule
{
    public interface IScheduleSocial
    {
        void FetchChannelJob(string platform, string user);
        void CreateChannelJob(string platfrom, string user);
        void UpdateChannelJob(string platform, string user, int id);
        (string, string) ValidateUrl(string url);
        
    }
}
