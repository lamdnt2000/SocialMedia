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

        public async Task<object> StatisticSubscription()
        {
            var total = await context.Subscriptions.Where(x => x.Status == 1).SumAsync(s => s.Price);
            var index = (int)DateTime.Now.DayOfWeek;
            var now = DateTime.Now.AddDays(1);
            var lastMonth = DateTime.Now.AddMonths(-1);
            var currentWeek = DateTime.Now.AddDays((index == 0) ? -6 : -(index - 1));
            var weekBefore = currentWeek.AddDays(-7);
            var profitInWeek = await context.Subscriptions.Where(x => x.Status == 1 
            && (x.CreatedDate>= currentWeek.Date && x.CreatedDate < now.Date)).SumAsync(s => s.Price);
            var profitLastWeek = await context.Subscriptions.Where(x => x.Status == 1 
            && (x.CreatedDate>= weekBefore.Date && x.CreatedDate < currentWeek.Date)).SumAsync(s => s.Price);
            var historyLastMonth = await context.Subscriptions.Where(x => x.Status == 1
            && (x.CreatedDate >= lastMonth.Date && x.CreatedDate < currentWeek.Date))
                .GroupBy(x => x.CreatedDate.Value.Date)
                .Select(x => new
                {
                    Key = x.Key,
                    Total = x.Sum(x=>x.Price)
                }).ToListAsync();
            var isIncreate = profitInWeek > profitLastWeek;
            var percent = (profitLastWeek == 0) ? 100 : Math.Round((float)Math.Abs(profitLastWeek - profitInWeek) / profitLastWeek * 100,2);
            return new
            {
                Total = total,
                ThisWeek = profitInWeek,
                LastWeek = profitLastWeek,
                IsIncreate = isIncreate,
                Percent = percent,
                History = historyLastMonth
            };
        }
    }
}
