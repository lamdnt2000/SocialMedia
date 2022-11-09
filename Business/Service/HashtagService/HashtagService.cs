using Business.Repository.HashtagRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.HashtagModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.HashtagService
{
    public class HashtagService : BaseService, IHashtagService
    {
        private readonly IHashtagRepository _hashtagRepository;

        private readonly string ClassName = typeof(Hashtag).Name;


        public HashtagService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IHashtagRepository hashtagRepository) : base(httpContextAccessor, userRepository)
        {
            _hashtagRepository = hashtagRepository;
        }

        public async Task<bool> Delete(int id)
        {
            var hashtag = await GetById(id);
            if (hashtag != null)
            {
                var result = await _hashtagRepository.Delete(id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<HashtagDto> GetById(int id)
        {
            var hashtag = await _hashtagRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<HashtagDto>(hashtag);
        }

        public async Task<int> Insert(InsertHashtagDto dto)
        {
            var check = await SearchByName(dto.Name);
            if (check == null)
            {
                var hashtag = MapperConfig.GetMapper().Map<Hashtag>(dto);
                return hashtag.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }

        public Task<bool> PagingSearch()
        {
            throw new NotImplementedException();
        }

        public async Task<HashtagDto> SearchByName(string name)
        {
            var hashtag = await _hashtagRepository.Get(x => x.Name.Equals(name));
            return MapperConfig.GetMapper().Map<HashtagDto>(hashtag);
        }

        public async Task<int> Update(int id, UpdateHashtagDto dto)
        {
            if ((await GetById(id) == null))
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }


            var check = await SearchByName(dto.Name);
            if (check == null || id == check.Id)
            {
                var hashtag = MapperConfig.GetMapper().Map<Hashtag>(dto);
                hashtag.Id = id;
                var result = await _hashtagRepository.Update(hashtag);
                return hashtag.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }
    }
}
