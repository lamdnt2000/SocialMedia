using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.LocationModel;
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
        Task<bool> Delete(int id);
        Task<ChannelCrawlDto> GetById(int id);
        Task<ChannelCrawlStatisticDto> Statistic(ChannelFilter filter);
    }
}
