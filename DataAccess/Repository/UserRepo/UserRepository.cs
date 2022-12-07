using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Enum;
using DataAccess.Models.LoginUser;
using System.Threading.Tasks;

namespace Business.Repository.UserRepo
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SocialMediaContext context) : base(context)
        {
        }
    }
}
