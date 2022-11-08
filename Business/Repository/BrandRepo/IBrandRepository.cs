using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.OrganizationModel;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.BrandRepo
{
    public interface IBrandRepository: IGenericRepository<Brand>
    {
        Task<PaginationList<Brand>> SearchAsync(BrandPaging paging);
    }
}
