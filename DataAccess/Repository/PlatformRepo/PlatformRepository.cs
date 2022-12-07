using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.PlatFormModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Repository.PlatformRepo
{
    public class PlatformRepository : GenericRepository<Platform>, IPlatformRepository
    {
        public PlatformRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<PaginationList<Platform>> SearchPlatformAsync(PlatformPaging paging)
        {
            var totalItem = await context.Platforms.ApplyFilterWithoutPagination(paging).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result =  context.Platforms.ApplyFilter(paging).ToList();
            return new PaginationList<Platform>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = (int)totalPage,
                Items = result
            };
        }
    }
}
