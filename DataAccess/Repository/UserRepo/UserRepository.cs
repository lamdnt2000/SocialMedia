using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Enum;
using DataAccess.Models.FeaturePlanModel;
using DataAccess.Models.LoginUser;
using DataAccess.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Repository.UserRepo
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<object> GetAllAsync()
        {
            return await context.Users.Select(u => new
            {
                Fullname = u.Firstname + " " + u.Lastname,
                Email = u.Email,
                CreatedDate = u.CreatedDate,
                Balance = (u.Wallet == null) ? 0 : u.Wallet.Balance,
                Provider = u.Provider,


            }).OrderByDescending(u=>u.CreatedDate).ToListAsync();
        }

        public async Task<object> SearchAsync(UserPaging paging)
        {
            var totalItem = await context.Users.ApplyFilterWithoutPagination(paging).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.Users.ApplyFilter(paging).Select(u => new
            {
                Fullname = u.Firstname + " " + u.Lastname,
                Email = u.Email,
                CreatedDate = u.CreatedDate,
                Balance = (u.Wallet == null) ? 0 : u.Wallet.Balance,
                Provider = u.Provider,


            }).AsEnumerable().Cast<dynamic>().ToList();
            return new PaginationList<object>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItem = totalItem,
                TotalPage = (int)totalPage,
                Items = result
            };
        }

        public async Task<object> StatisticCurrentUser(int uid)
        {
            var result = await context.Users.Where(u => u.Id == uid)
                .Select(x => new
                {
                    Balance = (x.Wallet == null) ? 0 : x.Wallet.Balance,
                    UserType = (x.UserType == null) ? "Free Trial" : x.UserType.Name,
                    ExpiresTime = x.UserType.DateEnd,
                    Features = JsonConvert.DeserializeObject<Dictionary<string, FeaturePlanDto>>(x.UserType.Feature)

                }).FirstOrDefaultAsync();
            var totalDeposit = await context.TransactionDeposits.Where(x => x.Wallet.UserId == uid && x.TransactionStatus == "00")
                .SumAsync(x => x.Amount);
            var totalBuying = await context.Subscriptions.Where(x => x.UserId == uid && x.Status == 1).SumAsync(x => x.Price);
            return new
            {
                user = result,
                totalDeposit = totalDeposit,
                totalBuying = totalBuying,
            };
        }

    }
}
