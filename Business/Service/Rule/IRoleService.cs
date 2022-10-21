using DataAccess.Models.Role;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Service.Rule
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetAllAsync();
        Task<int> Insert(InsertRoleDTO dto);
        Task<int> Update(UpdateRoleDTO dto);
        Task<int> Delete(int id);

    }
}
