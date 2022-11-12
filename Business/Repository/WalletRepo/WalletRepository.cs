using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.WalletModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.WalletRepo
{
    public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<PaginationList<Wallet>> SearchPlatformAsync(WalletPaging paging)
        {
            var totalItem = await context.Wallets.ApplyFilterWithoutPagination(paging).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.Wallets.ApplyFilter(paging).ToList();
            return new PaginationList<Wallet>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = (int)totalPage,
                Items = result
            };
        }
    }
}
