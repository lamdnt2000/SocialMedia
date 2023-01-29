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

        public async Task<object> MostWatchListChannel()
        {
            return context.Watchlists.Include(w => w.Channel).ThenInclude(c => c.Platform).ToList().GroupBy(x => x.ChannelId).Select(x => new
            {
                ChannelId = x.Key,
                Channel = x.Select(x => new
                {
                    Name = x.Channel.Name,
                    PlatformID = x.Channel.Platform.Name,
                    Avatar = x.Channel.AvatarUrl
                }).FirstOrDefault(),
                Count = x.Count()
            }).Take(5).ToList();
        }

        
        public async Task<object> PorfolioWatchListChannel(int uid)
        {
            return context.Watchlists.Where(x=> x.UserId == uid).Include(w => w.Channel).ThenInclude(c => c.Platform).ToList().GroupBy(x => x.Channel.PlatformId).Select(x => new
            {
                PlatformId = x.Key,
                PlatformName = x.Select(x=> x.Channel.Platform.Name).FirstOrDefault(),
                Count = x.Count()
            }).ToList();
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
