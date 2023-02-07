using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.OrganizationModel;
using DataAccess.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.BrandRepo
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<PaginationList<Brand>> SearchAsync(BrandPaging paging)
        {
            var totalItem = await context.Brands.ApplyFilterWithoutPagination(paging).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.Brands.ApplyFilter(paging).ToList();
            return new PaginationList<Brand>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItem = totalItem,
                TotalPage = (int)totalPage,
                Items = result
            };
        }

        public async Task<int> CountChannel(int id)
        {
            return await context.ChannelCrawls.Where(x => x.BrandId == id).CountAsync();
        }

         
    }
}
