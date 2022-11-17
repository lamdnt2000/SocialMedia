using Business.Repository.ChannelRecordRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.ChannelRecordModel;
using DataAccess.Models.Pagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.ChannelRecordService
{
    public class ChannelRecordService: BaseService, IChannelRecordService
    {
        private readonly IChannelRecordRepository _channelRecordRepository;
        private readonly string ClassName = typeof(ChannelRecordDto).Name;

        public ChannelRecordService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository
            , IChannelRecordRepository channelRecordRepository) : base(httpContextAccessor, userRepository)
        {
            _channelRecordRepository = channelRecordRepository;
        }

        public async Task<bool> Delete(int id)
        {
            var channelRecord = await GetById(id);
            if (channelRecord == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var result =await _userRepository.Delete(id);
            return result > 0;
        }

        public async Task<ChannelRecordDto> GetById(int id)
        {
            var channelRecord = await _channelRecordRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<ChannelRecordDto>(channelRecord);
        }

        public async Task<int> Insert(InsertChannelRecordDto dto)
        {
            var channelRecord = MapperConfig.GetMapper().Map<ChannelRecord>(dto);
            _channelRecordRepository.ValidEntity(channelRecord);
            
            channelRecord.Status = true;
            await _channelRecordRepository.Insert(channelRecord);
            return channelRecord.Id;
        }

        public Task<PaginationList<ChannelRecordDto>> SearchAsync(ChannelRecordPaging paging)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(int id, UpdateChannelRecordDto dto)
        {
            var check = await GetById(id);
            if (check == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var channelRecord = MapperConfig.GetMapper().Map<ChannelRecord>(dto);
            _channelRecordRepository.ValidEntity(channelRecord);
            channelRecord.Id = id;
          
            await _channelRecordRepository.Update(channelRecord);
            return channelRecord.Id;
        }
    }
}
