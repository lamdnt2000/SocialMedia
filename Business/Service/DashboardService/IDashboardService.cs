using DataAccess.Models.LoginUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.DashboardService
{
    public interface IDashboardService
    {
        Task<object> ShowDashBoardAdmin();
        Task<object> ShowDashboardUser();
        Task<object> SearchUserAsync(UserPaging paging);
    }
}
