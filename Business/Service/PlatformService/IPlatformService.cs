using DataAccess.Models.BranModel;
using DataAccess.Models.PlatFormModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.PlatformService
{
    public interface IPlatformService
    {
        Task<int> Insert(InsertPlatformDto dto);
        Task<int> Update(int id, UpdatePlatformDto dto);
        Task<bool> Delete(int id);
        Task<PlatformDto> GetById(int id, bool isInclude = false);
        Task<bool> PagingSearch();
        Task<PlatformDto> SearchByName(string name);
    }
}
