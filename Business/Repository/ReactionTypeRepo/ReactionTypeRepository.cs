using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFilterer.Extensions;
using Business.Repository.CategoryRepo;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.ReactionTypeModel;
using Microsoft.EntityFrameworkCore;

namespace Business.Repository.ReactionTypeRepo
{
    public class ReactionTypeRepository : GenericRepository<Reactiontype>, IReactionTypeRepository
    {
        public ReactionTypeRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<PaginationList<Reactiontype>> SearchAsync(ReactionTypePaging paging)
        {
            var totalItem = await context.Reactiontypes.ApplyFilterWithoutPagination(paging).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.Reactiontypes.ApplyFilter(paging).ToList();
            return new PaginationList<Reactiontype>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = (int)totalPage,
                Items = result
            };
        }
    }
}
