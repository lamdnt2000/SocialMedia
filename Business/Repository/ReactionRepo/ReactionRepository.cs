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

        public bool ValidEntity(Reaction reaction)
        {
            if (!(context.PostCrawls.Any(x => x.Id == reaction.PostId)))
            {
                throw new Exception("Post Crawl not exist!");
            }
            if (!(context.Reactiontypes.Any(x => x.Id == reaction.ReactionTypeId)))
            {
                throw new Exception("ReactionType not exist!");
            }
            return true;
        }
    }
}
