using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.SubscriptionModel;
using DataAccess.Models.TransectionDepositModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.SubscriptionRepo
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<PaginationList<Subscription>> SearchAsync(SubscriptionPaging paging)
        {
            var totalItem = await context.Subscriptions.ApplyFilterWithoutPagination(paging).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.Subscriptions.ApplyFilter(paging).ToList();
            return new PaginationList<Subscription>
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
