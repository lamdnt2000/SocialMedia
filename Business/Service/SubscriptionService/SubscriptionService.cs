using Business.Repository.SubscriptionRepo;
using Business.Repository.UserRepo;
using Business.Repository.WalletRepo;
using Business.Service.WalletService;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.SubscriptionModel;
using DataAccess.Models.TransectionDepositModel;
using DataAccess.Models.WalletModel;
using DataAccess.Repository.PlanRepo;
using DataAccess.Repository.UserTypeRepo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.SubscriptionService
{
    public class SubscriptionService : BaseService, ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IWalletRepository _walletRepository;

        private readonly string ClassName = typeof(SubscriptionDto).Name;
        private readonly string PlanName = typeof(Plan).Name;
        private readonly string WalletName = typeof(Wallet).Name;
        public SubscriptionService(
            IHttpContextAccessor httpContextAccessor
           , IUserRepository userRepository
           , ISubscriptionRepository subscriptionRepository
           , IPlanRepository planRepository
           , IWalletRepository walletRepository
           , IUserTypeRepository userTypeRepository) : base(httpContextAccessor, userRepository, userTypeRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _planRepository = planRepository;
            _walletRepository = walletRepository;
          
        }
        public async Task<bool> Delete(int id)
        {
            var channelRecord = await GetById(id);
            if (channelRecord == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var result = await _userRepository.Delete(id);
            return result > 0;
        }

        public async Task<SubscriptionDto> GetById(int id)
        {
            var subscription = await _subscriptionRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<SubscriptionDto>(subscription);
        }

        public async Task<int> Insert(PlanPaymentDto dto)
        {
            var plan = await _planRepository.Get(x => x.Id == dto.PlanId, new List<string>() { "PlanPrices" });
            if (plan == null)
            {
                throw new Exception(PlanName + " " + NOT_FOUND);
            }
            var price = plan.PlanPrices.Where(x => x.PriceType == dto.PriceType).FirstOrDefault();
            if (price == null)
            {
                throw new Exception("Price of plan " + INVALID);
            }
            Wallet wallet = await GetCurrentWallet();
            if (wallet.Balance< price.Price)
            {
                throw new Exception(WalletName + " " + NOT_ENOUGH + " balance");
            }
            Subscription subscription = new Subscription()
            {
                PlanId = dto.PlanId,
                Price = price.Price,
                UserId = GetCurrentUserId(),
                Status = 0,
                StartDate = DateTime.Now,
                WalletId = wallet.Id,
                PlanName = plan.Name
            };
            switch (dto.PriceType)
            {
                case 1:
                    subscription.EndDate = DateTime.Now.AddDays(price.Quantity * 7);
                    break;
                case 2:
                    subscription.EndDate = DateTime.Now.AddMonths(price.Quantity * 1);
                    break;
                case 3:
                    subscription.EndDate = DateTime.Now.AddYears(price.Quantity * 1);
                    break;
            }
            wallet.Balance -= (int)price.Price;
            var walletUpdate = await _walletRepository.Update(wallet);
            if (walletUpdate > 0)
            {
                subscription.Status = 1;
            }
            await _subscriptionRepository.Insert(subscription);
            
            if (subscription.Status == 1)
            {
                UserType userType = await GetCurrentUserType();
                if (userType == null)
                {
                    userType = new UserType()
                    {
                        Name = plan.Name,
                        DateStart = subscription.StartDate,
                        DateEnd = subscription.EndDate,
                        Valid = true,
                        UserId = GetCurrentUserId(),
                        SubId = subscription.Id,
                        PlanId = plan.Id,
                        Feature = await _planRepository.ConvertPlanFeatureToJson(dto.PlanId)
                    };
                    await AddUserType(userType);
                }
                else
                {
                    userType = new UserType()
                    {
                        Id = userType.Id,
                        Name = plan.Name,
                        DateStart = subscription.StartDate,
                        DateEnd = subscription.EndDate,
                        Valid = true,
                        UserId = GetCurrentUserId(),
                        SubId = subscription.Id,
                        PlanId = plan.Id,
                        Feature = await _planRepository.ConvertPlanFeatureToJson(dto.PlanId)
                    };
                    await UpdateUserType(userType);
                }
            }
            return subscription.Id;
        }

        public async Task<PaginationList<SubscriptionDto>> SearchAsync(SubscriptionPaging paging)
        {
            var wallet = await GetCurrentWallet();
            paging.WalletId = wallet.Id;
            var result = await _subscriptionRepository.SearchAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<SubscriptionDto>>(result.Items);
            return new PaginationList<SubscriptionDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalItem = result.TotalItem,
                TotalPage = result.TotalPage
            };
        }

      
        private async Task<Wallet> GetCurrentWallet()
        {
            int userId = GetCurrentUserId();

            var result = await _walletRepository.Get(x=> x.UserId == userId);
            if (result == null)
            {
                throw new Exception(WalletName + " " + NOT_FOUND);
            }
            return result;
        }
        
    }
}
