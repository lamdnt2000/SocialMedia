using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.GatewayModel;
using DataAccess.Models.Pagination;
using DataAccess.Models.PlatFormModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.GatewayRepo
{
    public interface IGatewayRepository : IGenericRepository<Gateway>
    {
        Task<PaginationList<Gateway>> SearchGatewayAsync(GatewayPaging paging);
    }
}
