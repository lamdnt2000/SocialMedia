using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.OrganizationModel;
using DataAccess.Models.Pagination;
using DataAccess.Models.WatchlistModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.WatchlistRepo
{
    public class WatchlistRepository : GenericRepository<Watchlist>, IWatchlistRepository
    {
        public WatchlistRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<PaginationList<Watchlist>> SearchAsync(string name, int platformId, WatchlistPaging paging, int userId)
        {
            var totalItem = await context.Watchlists.ApplyFilterWithoutPagination(paging).Where(x => x.UserId == userId && (name!=null? x.Channel.Name.ToLower().Contains(name): true) && x.Channel.PlatformId==platformId).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.Watchlists
                   .Include(x => x.Channel)
                   .ThenInclude(x=> x.Platform)
                   .Where(x => x.UserId == userId && (name != null ? x.Channel.Name.ToLower().Contains(name) : true) && x.Channel.Platform.Id==platformId)
                   .ToPaged(currentPage, pageSize)
                   .ToList();
            return new PaginationList<Watchlist>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = (int)totalPage,
                TotalItem = totalItem,
                Items = result
            };

        }
    }
}
