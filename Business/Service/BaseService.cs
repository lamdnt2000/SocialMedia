using System.Threading.Tasks;
using Business.Repository.UserRepo;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using static DataAccess.Enum.EnumConst;

namespace Business.Service
{
    public class BaseService
    {
        protected readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        protected BaseService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        protected string GetUserRole()
        {
            var user = _httpContextAccessor.HttpContext?.Items["User"];
            return user != null 
                ? user.GetType().GetProperty("role")?.GetValue(user, null)?.ToString()
                : string.Empty;
        }

        protected string GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.Items["User"];
            return user.GetType().GetProperty("id")?.GetValue(user, null).ToString();
        }
        
        protected string GetUserEmail()
        {
            var user = _httpContextAccessor.HttpContext?.Items["User"];
            return user.GetType().GetProperty("email")?.GetValue(user, null).ToString();
        }
        
        protected bool IsAdmin()
        {
            return GetUserRole().Equals(RoleEnum.ADMIN.ToString());
        }

        protected bool IsHotelOwner()
        {
            return GetUserRole().Equals(RoleEnum.MEMBER.ToString());
        }

        protected async Task<User> GetCurrentUser()
        {
            var userId = int.Parse(GetUserId());
            return await _userRepository.Get(user => user.Id == userId);
        }

        protected int GetCurrentUserId()
        {
            return int.Parse(GetUserId());
        } 
    }
}