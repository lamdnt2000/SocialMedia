using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.ChannelCrawlModel;
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
        Task<bool> BulkInsertOrUpdate(ChannelCrawl entity);
        Task<PaginationList<ChannelCrawl>> SearchAsync(ChannelSearchFilter paging);
    }
}
