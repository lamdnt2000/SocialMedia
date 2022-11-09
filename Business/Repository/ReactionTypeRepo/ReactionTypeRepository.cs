using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repository.CategoryRepo;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;

namespace Business.Repository.ReactionTypeRepo
{
    public class ReactionTypeRepository : GenericRepository<Reactiontype>, IReactionTypeRepository
    {
        public ReactionTypeRepository(SocialMediaContext context) : base(context)
        {
        }
    }
}
