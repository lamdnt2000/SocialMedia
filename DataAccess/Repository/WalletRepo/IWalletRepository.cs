using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.WalletModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.WalletRepo
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<PaginationList<Wallet>> SearchPlatformAsync(WalletPaging paging);
    }
}
