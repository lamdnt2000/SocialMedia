using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.LoginUser;
using System.Threading.Tasks;

namespace Business.Repository.UserRepo
{
    public interface IUserRepository : IGenericRepository<User>
    {
       
    }
}
