using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models.ReactionModel;

namespace Business.Service.ReactionService
{
    public interface IReactionService
    {
        Task<int> Insert(InsertReactionDto dto);
        Task<int> Update(int id, UpdateReactionDto dto);
        Task<bool> Delete(int id);
        Task<ReactionDto> GetById(int id);
        Task<bool> PagingSearch();
        Task<ReactionDto> SearchByName(string name);
    }
}
