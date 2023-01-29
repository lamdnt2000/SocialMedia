using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.LoginUser;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Repository.UserRepo
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<object> GetAllAsync();
        Task<object> SearchAsync(UserPaging paging);
        Task<object> StatisticCurrentUser(int uid);
    }
}
