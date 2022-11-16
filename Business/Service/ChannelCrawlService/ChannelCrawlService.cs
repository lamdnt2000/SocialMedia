using API.Utils;
using Business.Repository.ChannelCrawlRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.ChannelRecordModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;
using DateUtil = Business.Utils.DateUtil;

namespace Business.Service.ChannelCrawlService
{
    public class ChannelCrawlService : BaseService, IChannelCrawlService
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
            var record = MapperConfig.GetMapper().Map<ChannelRecord>(dto.ChannelRecord);
            _channelCrawlRepository.ValidateChannel(channel);
            channel.CreatedDate = DateTime.Now;
            channel.ChannelRecords.Add(record);
            await _channelCrawlRepository.Insert(channel);
            return channel.Id;
        }

        public async Task<ChannelCrawlStatisticDto> Statistic(ChannelFilter filter)
        {
            var channel = await _channelCrawlRepository.FilterChannel(filter);
            DateTime dateFrom = filter.CreatedTime.Min.Value;
            DateTime dateTo = filter.CreatedTime.Min.Value;
            double diff = DateUtil.DiffDate(dateFrom, dateTo);
            long follower = channel.ChannelRecords.Last().TotalFollower;
            var groupPost = channel.PostCrawls
                .Select(x => new
                {
                    x.Id,
                    x.CreatedTime,
                    Reactions = x.Reactions.Select(r => new
                    {
                        r.ReactionTypeId,
                        r.Count,
                        r.ReactionType.Name
                    })
                }).ToList()
                .GroupBy(x => x.CreatedTime.Value.Date).ToList();
            List<StatisticField> statistics = new List<StatisticField>();
            foreach (var post in groupPost)
            {
                StatisticField field = new StatisticField() { Date = post.Key };
                foreach (var item in post)
                {
                    foreach (var reaction in item.Reactions)
                    {

                        if (reaction.Name.Equals("reactionCare"))
                        {
                            field.TotalReactionCare += reaction.Count;
                        }
                        if (reaction.Name.Equals("reactionLike"))
                        {
                            field.TotalReactionLike += reaction.Count;
                        }
                        if (reaction.Name.Equals("reactionLove"))
                        {
                            field.TotalReactionLove += reaction.Count;
                        }
                        if (reaction.Name.Equals("reactionWow"))
                        {
                            field.TotalReactionWow += reaction.Count;
                        }
                        if (reaction.Name.Equals("reactionHAHA"))
                        {
                            field.TotalReactionHaha += reaction.Count;
                        }
                        if (reaction.Name.Equals("reactionSad"))
                        {
                            field.TotalReactionSad += reaction.Count;
                        }
                        if (reaction.Name.Equals("reactionAngry"))
                        {
                            field.TotalReactionAngry += reaction.Count;
                        }
                        if (reaction.Name.Equals("comment"))
                        {
                            field.TotalComment += reaction.Count;
                        }
                        if (reaction.Name.Equals("share"))
                        {
                            field.TotalShare += reaction.Count;
                        }
                        if (reaction.Name.Equals("totalLike"))
                        {
                            field.TotalReaction = reaction.Count;
                        }

                    }
                    field.TotalPost += 1;
                }
                field.AverageEngagementRate = Math.Round((float)(field.TotalReaction + field.TotalComment + field.TotalShare) / follower * 100, 4);
                statistics.Add(field);


            }
            var result = MapperConfig.GetMapper().Map<ChannelCrawlStatisticDto>(channel);
            result.StatisticFields = statistics;
            double difDate = DateUtil.DiffDate(result.CreatedTime, DateTime.Now);
            double difMonth = DateUtil.DiffMonth(result.CreatedTime, DateTime.Now);
            double difWeak = DateUtil.DiffWeek(result.CreatedTime, DateTime.Now);
            ChannelRecordDto recordDto = result.ChannelRecords.Last();
            long? totalRecord = recordDto.TotalLike + recordDto.TotalShare + recordDto.TotalComment;
            result.AveragePostInDay = Math.Round(recordDto.TotalPost / difDate, 2);
            result.AveragePostInMonth = Math.Round(recordDto.TotalPost / difMonth, 2);
            result.AveragePostInWeek = Math.Round(recordDto.TotalPost / difWeak, 2);
            result.AverageEngagementReactionRateInPost = (double)(totalRecord / recordDto.TotalPost);
            result.AverageEngagementLikeInPost = (double)(recordDto.TotalLike / recordDto.TotalPost);
            result.AverageEngagementCommentRateInPost = (double)(recordDto.TotalComment / recordDto.TotalPost);
            result.AverageEngagementShareRateInPost = (double)(recordDto.TotalShare / recordDto.TotalPost);
            return result;
        }

        public async Task<int> Update(int id, UpdateChannelCrawlDto dto)
        {
            var check = await _channelCrawlRepository.Get(x => x.Id == id);
            if (check == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var channel = MapperConfig.GetMapper().Map<ChannelCrawl>(dto);
            _channelCrawlRepository.ValidateChannel(channel);
            channel.Id = id;
            channel.UpdateDate = DateTime.Now;
            channel.CreatedDate = check.CreatedDate;
            await _channelCrawlRepository.Update(channel);
            return channel.Id;
        }

        /*public StatisticField DataProcess(IGrouping<DateTime, a> posts)
        {
            StatisticField statistic = new StatisticField() { Date = posts.Key.Date };
            foreach (var item in posts)
            {
                var reactions = item.Reactions.Select(x => new
                {
                    x.PostId,
                    x.ReactionTypeId,
                    x.Count,
                    x.ReactionType.Name
                }).ToArray();
            }
        }*/
    }
}
