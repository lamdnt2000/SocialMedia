using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.CategoryModel;
using DataAccess.Models.Pagination;
using DataAccess.Models.PlatFormModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.CategoryRepo
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<PaginationList<Category>> SearchAsync(CategoryPaging paging);
    }
}
