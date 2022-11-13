using DataAccess.Models.BranModel;
using DataAccess.Models.CategoryModel;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.CategoryService
{
    public interface ICategoryService
    {
        Task<long> Insert(InsertCategoryDto dto);
        Task<long> Update(long id, UpdateCategoryDto dto);
        Task<bool> Delete(long id);
        Task<CategoryDto> GetById(long id);
        Task<CategoryDto> SearchByName(string name);
        Task<PaginationList<CategoryDto>> SearchAsync(CategoryPaging paging);
    }
}
