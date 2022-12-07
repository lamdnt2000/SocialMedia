using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.LocationModel;
using DataAccess.Models.Pagination;
using System.Threading.Tasks;

namespace Business.Repository.LocationRepo
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        Task<PaginationList<Location>> SearchAsync(LocationPaging paging);
    }
}
