using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;

namespace Business.Repository.RoleRepo
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(SocialMediaContext context) : base(context)
        {
        }
    }
}
