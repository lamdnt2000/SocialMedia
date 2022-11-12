using Business.Repository.PostRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.PostCrawlModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;
namespace Business.Service.PostService
{
    public class PostCrawlService : BaseService, IPostCrawlService
    {
        private readonly IPostCrawlRepository _postCrawlRepository;
        private readonly string ClassName = typeof(PostCrawl).Name;

        public PostCrawlService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository
            , IPostCrawlRepository postCrawlRepository) : base(httpContextAccessor, userRepository)
        {
            _postCrawlRepository = postCrawlRepository;
        }

        public object ModelState { get; private set; }

        public async Task<bool> Delete(int id)
        {
            if (await GetById(id) == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var result = await _postCrawlRepository.Delete(id);
            return result > 0;
        }

        public async Task<PostCrawlDto> GetById(int id)
        {
            var post = await _postCrawlRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<PostCrawlDto>(post);
        }

        public async Task<int> Insert(InsertPostCrawlDto dto)
        {

            var post = MapperConfig.GetMapper().Map<PostCrawl>(dto);
            post.CreatedDate = DateTime.Now;
            post.Status = true;
            _postCrawlRepository.ValidEntity(post);
            var result = await _postCrawlRepository.Insert(post);
            return post.Id;
        }

        public async Task<int> Update(int id, UpdatePostCrawlDto dto)

        {
            var check = await GetById(id);
            if (check == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var post = MapperConfig.GetMapper().Map<PostCrawl>(dto);
            _postCrawlRepository.ValidEntity(post);
            post.Id = id;
            post.UpdateDate = DateTime.Now;
            post.CreatedDate = check.CreatedDate;
            var result = await _postCrawlRepository.Update(post);
            return post.Id;
        }
    }
}
