using Business.Repository.TransactionDepositRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Models.OrganizationModel;
using DataAccess.Models.Pagination;
using DataAccess.Models.TransectionDepositModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.TransactionDepositService
{
    public class TransactionDepositService:BaseService, ITransactionDepositService
    {
        private readonly ITransactionDepositRepository _transactionDepositRepository;

        public TransactionDepositService(IHttpContextAccessor httpContextAccessor
            , IUserRepository userRepository
            , ITransactionDepositRepository transactionDepositRepository) : base(httpContextAccessor, userRepository)
        {
            _transactionDepositRepository = transactionDepositRepository;
        }

    
    }
}
