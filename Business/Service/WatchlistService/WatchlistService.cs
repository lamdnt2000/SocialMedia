using Business.Repository.ChannelCrawlRepo;
using Business.Repository.UserRepo;
using Business.Repository.WatchlistRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.Pagination;
using DataAccess.Models.PlatFormModel;
using DataAccess.Models.WatchlistModel;
using DataAccess.Repository.UserTypeRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;


namespace Business.Service.WatchlistService
{
    public class WatchlistService : BaseService, IWatchlistService
    {
        private readonly IWatchlistRepository _watchlistRepository;
        private readonly string ClassName = typeof(Watchlist).Name;
        private readonly string ReferClassName = typeof(ChannelCrawl).Name;
        private readonly IChannelCrawlRepository _channelCrawlRepository;
        public WatchlistService(
            IHttpContextAccessor httpContextAccessor
            , IUserRepository userRepository
            , IUserTypeRepository userTypeRepository
            , IWatchlistRepository watchlistRepository
            , IChannelCrawlRepository channelCrawlRepository) :
            base(httpContextAccessor, userRepository, userTypeRepository)
        {
            _watchlistRepository = watchlistRepository;
            _channelCrawlRepository = channelCrawlRepository;
        }

        public async Task<bool> Delete(int channelId)
        {
            if (!(await ValidChannel(channelId)))
            {
                Watchlist watchlist = new Watchlist() { UserId = this.GetCurrentUserId(), ChannelId = channelId };
                await _watchlistRepository.Delete(this.GetCurrentUserId(), channelId);
                return true;
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<int> Insert(InsertWatchlistDto dto)
        {
            var exist = await _channelCrawlRepository.Get(x => x.Id == dto.ChannelId);
            if (exist == null)
            {
                throw new Exception(ReferClassName + " " + NOT_FOUND);
            }
            if (await ValidChannel(dto.ChannelId))
            {
                Watchlist watchlist = new Watchlist() { UserId = this.GetCurrentUserId(), ChannelId = dto.ChannelId };
                var result = await _watchlistRepository.Insert(watchlist);
                return result;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }

        public async Task<PaginationList<ChannelCrawlDto>> SearchAsync(string name, int platformId, WatchlistPaging paging)
        {
            var result = await _watchlistRepository.SearchAsync(name, platformId, paging, this.GetCurrentUserId());
            var item = MapperConfig.GetMapper().Map<List<ChannelCrawlDto>>(result.Items.Select(x=>x.Channel));
            return new PaginationList<ChannelCrawlDto>
            {
                Items = item,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalItem = result.TotalItem,
                TotalPage = result.TotalPage
            };

        }



        private async Task<bool> ValidChannel(int channelId)
        {

            var exist = await _watchlistRepository.Get(x => x.UserId == this.GetCurrentUserId() && x.ChannelId == channelId);
            return exist == null;
        }
    }
}
