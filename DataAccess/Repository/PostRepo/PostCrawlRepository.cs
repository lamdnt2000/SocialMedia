using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.PostRepo
{
    public class PostCrawlRepository : GenericRepository<PostCrawl>, IPostCrawlRepository
    {
        public PostCrawlRepository(SocialMediaContext context) : base(context)
        {
        }

        
    }
}
