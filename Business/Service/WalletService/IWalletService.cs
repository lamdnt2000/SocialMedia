using Business.Utils;
using DataAccess.Models.Pagination;
using DataAccess.Models.PaymentModel;
using DataAccess.Models.TransectionDepositModel;
using DataAccess.Models.WalletModel;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.WalletService
{
    public interface IWalletService
    {
        Task<int> Insert();
        Task<WalletDto> GetCurrentWallet();
        Task<int> UpdateBalance(VnPayLibrary vnpay);
        Task<int> DisableWallet(int userId);
        Task<bool> Delete(int id);
        Task<WalletDto> GetByUserId(int userId);
        Task<string> CreateDepositLink(PaymentDto dto);
        Task<PaginationList<TransactionDepositDto>> SearchTransaction(TransactionDepositPaging paging);
    }
}
