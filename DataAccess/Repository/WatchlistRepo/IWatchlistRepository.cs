using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.OrganizationModel;
using DataAccess.Models.Pagination;
using DataAccess.Models.WatchlistModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.WatchlistRepo
{
    public interface IWatchlistRepository: IGenericRepository<Watchlist>
    {
        Task<PaginationList<Watchlist>> SearchAsync(string name, int platformId, WatchlistPaging paging, int userId);
    }
}
