using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.OrganizationModel;
using System.Threading.Tasks;

namespace Business.Service.OrganizationService
{
    public interface IOrganizationService
    {
        Task<int> Insert(InsertOrganizationDto dto);
        Task<int> Update(UpdateOrganizationDto dto);
        Task<bool> Delete(int id);
        Task<OrganizationDto> GetById(int id);
        Task<bool> PagingSearch();
        Task<OrganizationDto> SearchByName(string name);
    }
}
