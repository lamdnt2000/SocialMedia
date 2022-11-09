using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.PlatFormModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Repository.PlatformRepo
{
    public interface IPlatformRepository: IGenericRepository<Platform>
    {
        Task<PaginationList<Platform>> SearchPlatformAsync(PlatformPaging paging);
    }
}
