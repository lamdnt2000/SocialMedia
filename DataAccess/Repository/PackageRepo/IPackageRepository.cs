using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.PackageModel;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.PackageRepo
{
    public interface IPackageRepository : IGenericRepository<Package>
    {
        Task<PaginationList<Package>> SearchPackageAsync(PakagePaging paging);
        Task<PackageDto> GetPlanOfPackage(int id);
        Task<ICollection<PackageDto>> GetAll();
    }
}
