using DataAccess.Entities;
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
        Task<User> GoogleSignUp(GoogleSignUpDto dto);
        Task<User> FacebookSignUp(FacebookSignUpDto dto);
        Task<User> GoogleSignIn(GoogleLoginDto dto);
        Task<User> FacebookSignIn(FacebookLoginDto dto);
    }
}
