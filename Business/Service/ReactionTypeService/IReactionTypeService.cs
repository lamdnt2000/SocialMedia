using DataAccess.Models.Pagination;
using DataAccess.Models.ReactionTypeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.ReactionTypeService
{
    public interface IReactionTypeService
    {
        Task<int> Insert(InsertReactionType dto);
        Task<int> Update(int id, UpdateReactionTypeDto dto);
        Task<bool> Delete(int id);
        Task<ReactionTypeDto> GetById(int id);
        Task<PaginationList<ReactionTypeDto>> SearchAsync(ReactionTypePaging paging);
        Task<ReactionTypeDto> SearchByName(string name);
    }
}
