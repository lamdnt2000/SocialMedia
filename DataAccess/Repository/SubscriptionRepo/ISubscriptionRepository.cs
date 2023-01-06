using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.SubscriptionModel;
using DataAccess.Models.TransectionDepositModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.SubscriptionRepo
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<PaginationList<Subscription>> SearchAsync(SubscriptionPaging paging);
    }
}
