using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using System.Threading.Tasks;
using System;
using DataAccess.Models.OrganizationModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Business.Repository.OrganizationRepo
{
    public class OrganizationRepository : GenericRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(SocialMediaContext context) : base(context)
        {

        }

        public async Task<int> CountChannel(int orgId)
        {
            return await context.ChannelCrawls.Where(x => x.OrganizationId == orgId).CountAsync();
        }

        public async Task<PaginationList<Organization>> SearchAsync(OrganizationPaging paging)
        {
            var totalItem = await context.Organizations.ApplyFilterWithoutPagination(paging).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.Organizations.ApplyFilter(paging).ToList();
            return new PaginationList<Organization>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = (int)totalPage,
                TotalItem = totalItem,
                Items = result
            };
        }
    }
}
