using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.CategoryModel;
using DataAccess.Models.OrganizationModel;
using DataAccess.Models.Pagination;
using System.Threading.Tasks;

namespace Business.Repository.OrganizationRepo
{
    public interface IOrganizationRepository : IGenericRepository<Organization>
    {
        Task<PaginationList<Organization>> SearchAsync(OrganizationPaging paging);
    }
}
