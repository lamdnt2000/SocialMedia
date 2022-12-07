using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.GatewayModel;
using DataAccess.Models.Pagination;
using DataAccess.Models.PlatFormModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.GatewayRepo
{
    public class GatewayRepository : GenericRepository<Gateway>, IGatewayRepository
    {
        public GatewayRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<PaginationList<Gateway>> SearchGatewayAsync(GatewayPaging paging)
        {
            var totalItem = await context.Gateways.ApplyFilterWithoutPagination(paging).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.Gateways.ApplyFilter(paging).ToList();
            return new PaginationList<Gateway>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = (int)totalPage,
                Items = result
            };
        }
    }
}
