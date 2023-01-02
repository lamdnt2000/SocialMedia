using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.ChannelCrawlModel.CompareModel;
using DataAccess.Models.LocationModel;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.ChannelCrawlService
{
    public interface IChannelCrawlService
    {
        Task<int> Insert(InsertChannelCrawlDto dto);
        Task<int> Update(int id, UpdateChannelCrawlDto dto);
        Task<int> Update(ChannelCrawlDto dto);
        Task<bool> Delete(int id);
        Task<ChannelCrawlDto> GetById(int id);
        Task<object> Statistic(ChannelFilter filter);
        Task<object> StatisticTopPost(int id);
        Task<string> FindChannelByPlatformAndUserId(string url);
        Task<PaginationList<ChannelCrawlDto>> SearchAsync(ChannelSearchFilter paging);
        Task<object> CompareChannel(CompareDto dto);
    }
}
