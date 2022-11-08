using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.BrandService
{
    public interface IBrandService
    {
        Task<int> Insert(InsertBrandDto dto);
        Task<int> Update(int id, UpdateBrandDto dto);
        Task<bool> Delete(int id);
        Task<BrandDto> GetById(int id);
        Task<PaginationList<BrandDto>> SearchAsync(BrandPaging paging);
        Task<BrandDto> SearchByName(string name);
    }
}
