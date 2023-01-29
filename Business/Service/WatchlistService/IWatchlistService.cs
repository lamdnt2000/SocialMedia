using DataAccess.Models.BranModel;
using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.Pagination;
using DataAccess.Models.WatchlistModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.WatchlistService
{
    public interface IWatchlistService
    {
        Task<int> Insert(InsertWatchlistDto dto);
        Task<bool> Delete(int channelId);
       
        Task<PaginationList<ChannelCrawlDto>> SearchAsync(string name,int platformId, WatchlistPaging paging);
    }
}
