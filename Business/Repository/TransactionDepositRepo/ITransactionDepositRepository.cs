﻿using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.TransectionDepositModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.TransactionDepositRepo
{
    public interface ITransactionDepositRepository : IGenericRepository<TransactionDeposit>
    {
        Task<PaginationList<TransactionDeposit>> SearchAsync(TransactionDepositPaging paging);
    }
}