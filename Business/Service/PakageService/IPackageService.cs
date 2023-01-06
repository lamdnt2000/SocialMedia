using DataAccess.Models.PackageModel;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.PakageService
{
    public interface IPackageService
    {
        Task<int> Insert(InsertPakageDto dto);
        Task<int> Update(int id, UpdatePakageDto dto);
        Task<bool> Delete(int id);
        Task<PackageDto> GetById(int id);
        Task<PackageDto> GetPackageInclude(int id);
        Task<PackageDto> GetPlansOfPackage(int id);
        Task<PaginationList<PackageDto>> SearchAsync(PakagePaging paging);
        Task<PackageDto> SearchByName(string name);
    }
}
