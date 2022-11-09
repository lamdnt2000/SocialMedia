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
        Task<int> Insert(InsertCategoryDto dto);
        Task<int> Update(int id, UpdateCategoryDto dto);
        Task<bool> Delete(int id);
        Task<CategoryDto> GetById(int id);
        Task<CategoryDto> SearchByName(string name);
        Task<PaginationList<CategoryDto>> SearchAsync(CategoryPaging paging);
    }
}
