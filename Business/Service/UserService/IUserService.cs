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

        Task<User> FacebookSignUp(FacebookSignUpDto dto);
        Task<User> GoogleSignIn(string TokenId);
        Task<User> FacebookSignIn(FacebookLoginDto dto);
        Task<bool> UpdateUserInformation(UpdateUserDto dto);
        Task<bool> UpdateUserPassword(UpdateUserPassworDto dto);
        Task<UpdateUserDto> GetCurrentUserProfile();
    }
}
