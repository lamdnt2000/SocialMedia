using DataAccess.Models.Pagination;
using DataAccess.Models.WalletModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.WalletService
{
    public interface IWalletService
    {
        Task<int> Insert(InsertWalletDto dto);
        Task<int> Update(int id, UpdateWalletDto dto);
        Task<bool> Delete(int id);
        Task<WalletDto> GetById(int id, bool isInclude = false);
        Task<PaginationList<WalletDto>> SearchAsync(WalletPaging paging);
        Task<WalletDto> SearchByCurrency(string currency);
    }
}
