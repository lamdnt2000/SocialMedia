using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.PostCrawlModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.PostService
{
    public interface IPostCrawlService
    {
        Task<int> Insert(InsertPostCrawlDto dto);
        Task<int> Update(int id, UpdatePostCrawlDto dto);
        Task<bool> Delete(int id);
        Task<PostCrawlDto> GetById(int id);
    }
}
