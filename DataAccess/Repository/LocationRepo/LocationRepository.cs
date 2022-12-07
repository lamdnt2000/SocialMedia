using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.LocationModel;
using DataAccess.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Repository.LocationRepo
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<PaginationList<Location>> SearchAsync(LocationPaging paging)
        {
            var totalItem = await context.Locations.ApplyFilterWithoutPagination(paging).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.Locations.ApplyFilter(paging).ToList();
            return new PaginationList<Location>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = (int)totalPage,
                Items = result
            };
        }
    }
}
