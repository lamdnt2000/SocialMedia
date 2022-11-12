using Business.Repository.UserRepo;
using Business.Repository.WalletRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.WalletModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.WalletService
{
    public class WalletService : BaseService, IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        private readonly string ClassName = typeof(Wallet).Name;

        public WalletService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IWalletRepository walletRepository) : base(httpContextAccessor, userRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<bool> Delete(int id)
        {
            var wallet = await GetById(id);
            if (wallet != null)
            {
                var result = await _walletRepository.Delete(id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<WalletDto> GetById(int id, bool isInclude = false)
        {
            var wallet = await _walletRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<WalletDto>(wallet);
        }

        public async Task<int> Insert(InsertWalletDto dto)
        {
            var check = await SearchByCurrency(dto.Currency);
            if (check == null)
            {
                var wallet = MapperConfig.GetMapper().Map<Wallet>(dto);
                var result = await _walletRepository.Insert(wallet);
                return wallet.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }

        public async Task<PaginationList<WalletDto>> SearchAsync(WalletPaging paging)
        {
            var result = await _walletRepository.SearchPlatformAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<WalletDto>>(result.Items);
            return new PaginationList<WalletDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalPage = result.TotalPage
            };
        }

        public async Task<WalletDto> SearchByCurrency(string currency)
        {
            var platform = await _walletRepository.Get(x => x.Currency.Equals(currency));
            return MapperConfig.GetMapper().Map<WalletDto>(platform);
        }

        public async Task<int> Update(int id, UpdateWalletDto dto)
        {
            var check = await SearchByCurrency(dto.Currency);
            if ((await GetById(id) == null))
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            if (check == null || id == check.Id)
            {
                var wallet = MapperConfig.GetMapper().Map<Wallet>(dto);
                wallet.Id = id;
                var result = await _walletRepository.Update(wallet);
                return wallet.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }
    }
}
