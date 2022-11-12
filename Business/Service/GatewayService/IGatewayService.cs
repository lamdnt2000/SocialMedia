using DataAccess.Models.GatewayModel;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.GatewayService
{
    public interface IGatewayService
    {
        Task<int> Insert(InsertGatewayDto dto);
        Task<int> Update(int id, UpdateGatewayDto dto);
        Task<bool> Delete(int id);
        Task<GatewayDto> GetById(int id, bool isInclude = false);
        Task<PaginationList<GatewayDto>> SearchAsync(GatewayPaging paging);
        Task<GatewayDto> SearchByName(string name);
    }
}
