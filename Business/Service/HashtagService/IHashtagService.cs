using DataAccess.Models.HashtagModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.HashtagService
{
    public interface IHashtagService 
    {
        Task<int> Insert(InsertHashtagDto dto);
        Task<int> Update(int id, UpdateHashtagDto dto);
        Task<bool> Delete(int id);
        Task<HashtagDto> GetById(int id);
        Task<bool> PagingSearch();
        Task<HashtagDto> SearchByName(string name);
    }
}
