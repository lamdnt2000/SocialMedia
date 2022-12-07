using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.ReactionTypeModel;

namespace Business.Repository.ReactionTypeRepo
{
    public interface IReactionTypeRepository : IGenericRepository<Reactiontype>
    {
        Task<PaginationList<Reactiontype>> SearchAsync(ReactionTypePaging paging);
    }
}
