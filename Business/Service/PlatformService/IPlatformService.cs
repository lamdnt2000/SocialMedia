using DataAccess.Models.Pagination;
using DataAccess.Models.PlatFormModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Service.PlatformService
{
    public interface IPlatformService
    {
        Task<int> Insert(InsertPlatformDto dto);
        Task<int> Update(int id, UpdatePlatformDto dto);
        Task<bool> Delete(int id);
        Task<PlatformDto> GetById(int id, bool isInclude = false);
        Task<PaginationList<PlatformDto>> SearchAsync(PlatformPaging paging);
        Task<PlatformDto> SearchByName(string name);
    }
}
