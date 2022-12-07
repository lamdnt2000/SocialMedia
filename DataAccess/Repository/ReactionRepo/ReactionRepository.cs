using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.ReactionRepo
{
    public class ReactionRepository:GenericRepository<Reaction>, IReactionRepository
    {
        public ReactionRepository(SocialMediaContext context) : base(context)
        {
        }

      
    }
}
