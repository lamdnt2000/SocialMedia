using AutoFilterer.Types;
using DataAccess.Models.BranModel;
using DataAccess.Models.LocationModel;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.LocationService
{
    public interface ILocationService
    {
        Task<int> Insert(InsertLocationDto dto);
        Task<int> Update(int id, UpdateLocationDto dto);
        Task<bool> Delete(int id);
        Task<LocationDto> GetById(int id);
        Task<PaginationList<LocationDto>> SearchAsync(LocationPaging paging);
    }
}
