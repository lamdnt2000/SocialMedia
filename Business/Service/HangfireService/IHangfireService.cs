using DataAccess.Entities;
using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Business.Service.HangfireService
{
    public interface IHangfireService
    {
        IEnumerable<object> GetCurrentConcurrentJob();
        Task<PaginationList<ChannelCrawlDto>> GetAllChannelSchedule(HangfireChannelFilter filter);
        Task<IEnumerable<ChannelCrawl>> CreateSchedule(List<int> ids);
        string TriggerJob(string jobId);
        void DeleteJob(string jobId);
    }
}
