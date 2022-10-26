using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;

namespace Business.Repository.UserRepo
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SocialMediaContext context) : base(context)
        {
        }
    }
}
