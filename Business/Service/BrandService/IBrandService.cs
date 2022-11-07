using DataAccess.Entities;
using DataAccess.Models.BranModel;
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
        Task<bool> PagingSearch();
        Task<BrandDto> SearchByName(string name);
    }
}
