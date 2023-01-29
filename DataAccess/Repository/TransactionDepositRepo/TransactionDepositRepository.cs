using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.TransectionDepositModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.TransactionDepositRepo
{
    public class TransactionDepositRepository : GenericRepository<TransactionDeposit>, ITransactionDepositRepository
    {
        public TransactionDepositRepository(SocialMediaContext context) : base(context)
        {
        }
        public async Task<PaginationList<TransactionDeposit>> SearchAsync(TransactionDepositPaging paging)
        {
            var totalItem = await context.TransactionDeposits.ApplyFilterWithoutPagination(paging).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.TransactionDeposits.ApplyFilter(paging).ToList();
            return new PaginationList<TransactionDeposit>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItem = totalItem,
                TotalPage = (int)totalPage,
                Items = result
            };
        }

        public async Task<object> StatisticDeposit()
        {
            var total = await context.TransactionDeposits.Where(x => x.TransactionStatus == "00").SumAsync(s => s.Amount);
            var index = (int)DateTime.Now.DayOfWeek;
            var now = DateTime.Now.AddDays(1);
            var lastMonth = DateTime.Now.AddMonths(-1);
            var currentWeek = DateTime.Now.AddDays((index == 0) ? -6 : -(index - 1));
            var weekBefore = currentWeek.AddDays(-7);
            var profitInWeek = await context.TransactionDeposits.Where(x => x.TransactionStatus == "00"
            && (x.PayDate >= currentWeek.Date && x.PayDate < now.Date)).SumAsync(s => s.Amount);
            var profitLastWeek = await context.TransactionDeposits.Where(x => x.TransactionStatus == "00"
            && (x.PayDate >= weekBefore.Date && x.PayDate < currentWeek.Date)).SumAsync(s => s.Amount);
            var historyLastMonth = await context.TransactionDeposits.Where(x => x.TransactionStatus == "00"
            && (x.PayDate >= lastMonth.Date && x.PayDate < currentWeek.Date))
                .GroupBy(x => x.PayDate.Value.Date)
                .Select(x => new
                {
                    Key = x.Key,
                    Total = x.Sum(x => x.Amount)
                }).ToListAsync();
            var isIncreate = profitInWeek.Value > profitLastWeek.Value;
            var percent = (profitLastWeek.Value == 0) ? 100 : Math.Round((float)Math.Abs(profitLastWeek.Value - profitInWeek.Value) / profitLastWeek.Value * 100, 2);
            return new
            {
                Total = total.Value,
                ThisWeek = profitInWeek.Value,
                LastWeek = profitLastWeek.Value,
                IsIncreate = isIncreate,
                Percent = percent,
                History = historyLastMonth
            };
        }
    }
}
