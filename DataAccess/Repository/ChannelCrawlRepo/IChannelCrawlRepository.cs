using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.ChannelCrawlModel.FacebookStatistic;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.ChannelCrawlRepo
{
    public interface IChannelCrawlRepository: IGenericRepository<ChannelCrawl>
    {
        Task<bool> ValidateChannelAsync(ChannelCrawl entity);
        Task<ChannelStatistic> FilterChannel(ChannelFilter filter);
        Task<object> FilterTopPost(int id);
        Task<Dictionary<string, object>> FilterTopPostFacebookChannel(int id);
        Task<Dictionary<string, object>> FilterTopPostTiktokChannel(int id);
        Task<Dictionary<string, object>> FilterTopPostYoutubeChannel(int id);
        Task<PaginationList<ChannelCrawl>> GetChannelSchedule(List<string> concurrentJobs, HangfireChannelFilter filter);
        Task<bool> BulkInsertOrUpdate(ChannelCrawl entity);
        Task<PaginationList<ChannelCrawl>> SearchAsync(ChannelSearchFilter paging);
        Task<List<ChannelStatistic>> FilterChannels(List<string> userIds, ChannelFilter filter);
        Task<object> StatisticChannel();
        Task<object> StatisticDashboard();
    }
}
