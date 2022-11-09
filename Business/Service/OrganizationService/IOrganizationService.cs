using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.OrganizationModel;
using DataAccess.Models.Pagination;
using System.Threading.Tasks;

namespace Business.Service.OrganizationService
{
    public interface IOrganizationService
    {
        Task<int> Insert(InsertOrganizationDto dto);
        Task<int> Update(int id, UpdateOrganizationDto dto);
        Task<bool> Delete(int id);
        Task<OrganizationDto> GetById(int id, bool isInclude = false);
        Task<PaginationList<OrganizationDto>> SearchAsync(OrganizationPaging paging);
        Task<OrganizationDto> SearchByName(string name);
    }
}
