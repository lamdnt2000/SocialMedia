using Business.Repository.ChannelCrawlRepo;
using Business.Repository.SubscriptionRepo;
using Business.Repository.TransactionDepositRepo;
using Business.Repository.UserRepo;
using Business.Repository.WatchlistRepo;
using Business.Service.WatchlistService;
using DataAccess.Models.LoginUser;
using DataAccess.Repository.UserTypeRepo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.DashboardService
{
    public class DashboardService : BaseService, IDashboardService
    {
        private readonly IChannelCrawlRepository _channelCrawlRepository;
        private readonly ISubscriptionRepository _subscriptionService;
        private readonly IUserRepository _userRepository;
        private readonly IWatchlistRepository _watchlistRepository;
        private readonly ITransactionDepositRepository _transactionDepositRepository;

        public DashboardService(IHttpContextAccessor httpContextAccessor
            , IUserRepository userRepository
            , IUserTypeRepository userTypeRepository
            , IChannelCrawlRepository channelCrawlRepository
            , ISubscriptionRepository subscriptionService
            , IWatchlistRepository watchlistRepository
            , ITransactionDepositRepository transactionDepositRepository) : base(httpContextAccessor, userRepository, userTypeRepository)
        {
            _channelCrawlRepository = channelCrawlRepository;
            _subscriptionService = subscriptionService;
            _userRepository = userRepository;
            _watchlistRepository = watchlistRepository;
            _transactionDepositRepository = transactionDepositRepository;
        }
        public async Task<object> ShowDashBoardAdmin()
        {
            var channel = await StatisticChannel();
            var watchList = await StatisticChannelWatchList();
            var profit = await StatisticSubscription();
            var deposits = await StatisticDepositMoney();
            var crawls = await StatisticChannelCrawl();
            return new
            {
                channels = channel,
                watchlists = watchList,
                profit = profit,
                deposits = deposits,
                crawls = crawls
            };
        }

        private async Task<object> StatisticChannel()
        {
            return await _channelCrawlRepository.StatisticChannel();
        }
        private async Task<object> StatisticSubscription()
        {
            return await _subscriptionService.StatisticSubscription();
        }
        private async Task<object> StatisticDepositMoney()
        {
            return await _transactionDepositRepository.StatisticDeposit();
        }
        private async Task<object> StatisticUser()
        {
            return await _userRepository.GetAllAsync();
        }
        private async Task<object> StatisticChannelWatchList()
        {
            return await _watchlistRepository.MostWatchListChannel();
        }

        public async Task<object> SearchUserAsync(UserPaging paging)
        {
            return await _userRepository.SearchAsync(paging);
        }
        public async Task<object> StatisticChannelCrawl()
        {
            return await _channelCrawlRepository.StatisticDashboard();
        }

        public async Task<object> ShowDashboardUser()
        {
            var channel = await StatisticChannel();

            var watchList = await StatisticChannelWatchList();
            var porfolio = await PortfolioWhatchList();
            var user = await StatisticCurrentUser();
            return new
            {
                channels = channel,
                watchlists = watchList,
                porfolio = porfolio,
                user

            };
        }

        private async Task<object> PortfolioWhatchList()
        {
            int uid = GetCurrentUserId();
            return await _watchlistRepository.PorfolioWatchListChannel(uid);
        }

        private async Task<object> StatisticCurrentUser()
        {
            int uid = GetCurrentUserId();
            return await _userRepository.StatisticCurrentUser(uid);
        }
    }
}
