using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;

namespace Business.Repository.OrganizationRepo
{
    public class OrganizationRepository : GenericRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(SocialMediaContext context) : base(context)
        {

        }
    }
}
