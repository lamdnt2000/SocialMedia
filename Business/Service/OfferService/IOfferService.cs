using DataAccess.Models.OfferModel;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.OfferService
{
    public interface IOfferService
    {
        Task<int> Insert(InsertOfferDto dto);
        Task<int> Update(int id, UpdateOfferDto dto);
        Task<bool> Delete(int id);
        Task<OfferDto> GetById(int id);
        Task<PaginationList<OfferDto>> SearchAsync(OfferPaging paging);
        Task<OfferDto> SearchByName(string name);
    }
}
