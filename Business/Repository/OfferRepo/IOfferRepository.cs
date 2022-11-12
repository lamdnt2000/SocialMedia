using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.OfferModel;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.OfferRepo
{
    public interface IOfferRepository : IGenericRepository<Offer>
    {
        Task<PaginationList<Offer>> SearchPlatformAsync(OfferPaging paging);
    }
}
