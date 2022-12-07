using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.PackageModel;
using DataAccess.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.PackageRepo
{
    public class PackageRepository : GenericRepository<Package>, IPackageRepository
    {
        public PackageRepository(SocialMediaContext context) : base(context)
        {
        }
        public async Task<PaginationList<Package>> SearchPackageAsync(PakagePaging paging)
        {
            var totalItem = await context.Packages.ApplyFilterWithoutPagination(paging).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.Packages.ApplyFilter(paging).ToList();
            return new PaginationList<Package>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = (int)totalPage,
                Items = result
            };
        }
    }
}
