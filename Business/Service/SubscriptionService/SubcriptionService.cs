using Business.Repository.SubscriptionRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.SubscriptionModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.SubscriptionService
{
    public class SubcriptionService : BaseService, ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly string ClassName = typeof(SubscriptionDto).Name;
        public SubcriptionService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository
           , ISubscriptionRepository subscriptionRepository) : base(httpContextAccessor, userRepository)
        {
            _subscriptionRepository = subscriptionRepository;
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

        public async Task<int> Insert(InsertSubscriptionDto dto)
        {
            var subscription = MapperConfig.GetMapper().Map<Subscription>(dto);
            _subscriptionRepository.ValidateCSubscription(subscription);
            subscription.CreateDate = DateTime.Now;
            await _subscriptionRepository.Insert(subscription);
            return subscription.Id;
        }

        public async Task<PaginationList<SubscriptionDto>> SearchAsync(SubscriptionPaging paging)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(int id, UpdateSubscriptionDto dto)
        {
            if (await GetById(id) == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var subscription = MapperConfig.GetMapper().Map<Subscription>(dto);
            _subscriptionRepository.ValidateCSubscription(subscription);
            subscription.UpdateDate = DateTime.Now;
            await _subscriptionRepository.Update(subscription);
            return subscription.Id;
        }
    }
}
