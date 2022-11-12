using DataAccess.Models.Pagination;
using DataAccess.Models.TransectionDepositModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.TransactiondepositService
{
    public interface ITransactiondepositService
    {
        Task<int> Insert(InsertTransactionDepositDto dto);
        Task<int> Update(int id, UpdateTransactionDepositDto  dto);
        Task<bool> Delete(int id);
        Task<TransactionDepositDto> GetById(int id, bool isInclude = false);
        Task<PaginationList<TransactionDepositDto>> SearchAsync(TransactionDepositPaging paging);
        Task<TransactionDepositDto> SearchByCode(string code);
    }
}
