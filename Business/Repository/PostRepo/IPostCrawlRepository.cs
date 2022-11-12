using Business.Repository.GenericRepo;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.PostRepo
{
    public interface IPostCrawlRepository:IGenericRepository<PostCrawl>
    {
        bool ValidEntity(PostCrawl entity);
    }
}
