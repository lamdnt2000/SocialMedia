using Business.Repository.ChannelCrawlRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.LocationModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;
namespace Business.Service.ChannelCrawlService
{
    public class ChannelCrawlService:BaseService, IChannelCrawlService
    {
        private readonly IChannelCrawlRepository _channelCrawlRepository;
        private string ClassName = typeof(ChannelCrawl).Name;
        public ChannelCrawlService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository
            , IChannelCrawlRepository channelCrawlRepository) : base(httpContextAccessor, userRepository)
        {
            _channelCrawlRepository = channelCrawlRepository;
        }

        public async Task<bool> Delete(int id)
        {
            var channel = await GetById(id);
            if (channel != null)
            {
                var result = await _channelCrawlRepository.Delete(id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<ChannelCrawlDto> GetById(int id)
        {
            var chanel = await _channelCrawlRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<ChannelCrawlDto>(chanel);
        }

        public async Task<int> Insert(InsertChannelCrawlDto dto)
        {
            var channel = MapperConfig.GetMapper().Map<ChannelCrawl>(dto);
            _channelCrawlRepository.ValidateChannel(channel);
            channel.CreatedDate = DateTime.Now;
            await _channelCrawlRepository.Insert(channel);
            return channel.Id;
        }

        public async Task<int> Update(int id, UpdateChannelCrawlDto dto)
        {
            if ((await _channelCrawlRepository.Get(x => x.Id == id)) == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var channel = MapperConfig.GetMapper().Map<ChannelCrawl>(dto);
            channel.Id = id;
             _channelCrawlRepository.ValidateChannel(channel);
            channel.UpdateDate = DateTime.Now;
            await _channelCrawlRepository.Update(channel);
            return channel.Id;
        }
    }
}
