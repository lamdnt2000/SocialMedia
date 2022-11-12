using Business.Repository.GatewayRepo;
using Business.Repository.TransactionDepositRepo;
using Business.Repository.UserRepo;
using Business.Repository.WalletRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.TransectionDepositModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.TransactiondepositService
{
    public class TransactiondepositService : BaseService, ITransactiondepositService
    {
        private readonly ITransactionDepositRepository _transactionDepositRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IGatewayRepository _gatewayRepository;
        private readonly string ClassName = typeof(TransactionDeposit).Name;
        private readonly string ReferClassName1 = typeof(Wallet).Name;
        private readonly string ReferClassName2 = typeof(Gateway).Name;

        public TransactiondepositService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            ITransactionDepositRepository transactionDepositRepository,
            IWalletRepository walletRepository,
            IGatewayRepository gatewayRepository) : base(httpContextAccessor, userRepository)
        {
            _transactionDepositRepository = transactionDepositRepository;
            _walletRepository = walletRepository;
            _gatewayRepository = gatewayRepository;
        }
        public async Task<bool> Delete(int id)
        {
            var transactionDeposit = await GetById(id);
            if (transactionDeposit != null)
            {
                var result = await _transactionDepositRepository.Delete(id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<TransactionDepositDto> GetById(int id, bool isInclude = false)
        {
            var transactionDeposit = await _transactionDepositRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<TransactionDepositDto>(transactionDeposit);
        }

        public async Task<int> Insert(InsertTransactionDepositDto dto)
        {
            if (!(await ValidWallet(dto.WalletId)))
            {
                throw new Exception(ReferClassName1 + " " + NOT_FOUND);
            }
            if (!(await ValidGateway(dto.GatewayId)))
            {
                throw new Exception(ReferClassName2 + " " + NOT_FOUND);
            }
            var check = await SearchByCode(dto.Code);
            if (check == null)
            {
                var transactionDeposit = MapperConfig.GetMapper().Map<TransactionDeposit>(dto);
                var result = await _transactionDepositRepository.Insert(transactionDeposit);
                return transactionDeposit.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            };
        }

        private async Task<bool> ValidWallet(int id)
        {
            var wallet = await _transactionDepositRepository.Get(x => x.Id == id);
            return wallet != null;
        }

        private async Task<bool> ValidGateway(int id)
        {
            var gateway = await _transactionDepositRepository.Get(x => x.Id == id);
            return gateway != null;
        }

        public async Task<PaginationList<TransactionDepositDto>> SearchAsync(TransactionDepositPaging paging)
        {
            var result = await _transactionDepositRepository.SearchAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<TransactionDepositDto>>(result.Items);
            return new PaginationList<TransactionDepositDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalPage = result.TotalPage
            };
        }

        public async Task<TransactionDepositDto> SearchByCode(string code)
        {
            var transactionDeposit = await _transactionDepositRepository.Get(x => x.Code.Equals(code));
            return MapperConfig.GetMapper().Map<TransactionDepositDto>(transactionDeposit);
        }

        public async Task<int> Update(int id, UpdateTransactionDepositDto dto)
        {
            if ((await GetById(id) == null))
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }

            if (!(await ValidWallet(dto.WalletId)))
            {
                throw new Exception(ReferClassName1 + " " + NOT_FOUND);
            }
            if (!(await ValidGateway(dto.GatewayId)))
            {
                throw new Exception(ReferClassName2 + " " + NOT_FOUND);
            }

            var check = await SearchByCode(dto.Code);
            if (check == null || id == check.Id)
            {
                var transactionDeposit = MapperConfig.GetMapper().Map<TransactionDeposit>(dto);
                transactionDeposit.Id = id;
                var result = await _transactionDepositRepository.Update(transactionDeposit);
                return transactionDeposit.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }
    }
}
