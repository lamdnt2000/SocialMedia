using DataAccess;
using DataAccess.Models.LoginUser;
using System.Threading.Tasks;

namespace Business.Service.UserService
{
    public interface IUserService
    {
        Task<User> FindByEmail(string email);
        bool CheckPassword(string inputPassword, string currentPassword);
        Task<int> Insert(InsertUserDTO user);
        Task<User> GetUser(LoginUserDTO user);
        Task<int> Update(InsertUserDTO user);
    }
}
