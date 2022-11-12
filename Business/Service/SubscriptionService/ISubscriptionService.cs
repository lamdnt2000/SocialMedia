using DataAccess.Models.Pagination;
using DataAccess.Models.SubscriptionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.SubscriptionService
{
    public interface ISubscriptionService
    {
        Task<int> Insert(InsertSubscriptionDto dto);
        Task<int> Update(int id, UpdateSubscriptionDto dto);
        Task<bool> Delete(int id);
        Task<SubscriptionDto> GetById(int id);
        Task<PaginationList<SubscriptionDto>> SearchAsync(SubscriptionPaging paging);
    }
}
