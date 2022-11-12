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
                TotalPage = (int)totalPage,
                Items = result
            };
        }
    }
}
