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
using static Business.Schedule.RegexUtil;
using static Business.Schedule.TriggerUtil;
using static Business.Constants.PlatFormEnum;
using DataAccess.Enum;
using Microsoft.AspNetCore.Mvc;
using Business.Constants;
using Business.Schedule;
using Hangfire;

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
            await _channelCrawlRepository.ValidateChannelAsync(channel);
            channel.ChannelRecords.Add(record);
            await _channelCrawlRepository.BulkInsertOrUpdate(channel);
            return channel.Id;
        }

        public async Task<object> Statistic(ChannelFilter filter)
        {
            var channel = await _channelCrawlRepository.FilterChannel(filter);
            int platform = channel.PlatformId;
            switch (platform)
            {
                case (int)EnumConst.PlatFormEnum.FACEBOOK:
                    {
                        return FacebookStatistic(channel, filter);
                    }
                case (int)EnumConst.PlatFormEnum.YOUTUBE:
                    {
                        return YoutubeStatistic(channel, filter);
                    }
                case (int)EnumConst.PlatFormEnum.TIKTOK:
                    {
                        return TiktokStatistic(channel, filter);
                    }
                default: return null;

            }
        }

        public async Task<int> Update(int id, UpdateChannelCrawlDto dto)
        {
            var check = await _channelCrawlRepository.Get(x => x.Id == id);
            if (check == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var channel = MapperConfig.GetMapper().Map<ChannelCrawl>(dto);
            var record = MapperConfig.GetMapper().Map<ChannelRecord>(dto.ChannelRecord);
            channel.Id = id;
            channel.ChannelRecords.Add(record);
            await _channelCrawlRepository.ValidateChannelAsync(channel);
            
            await _channelCrawlRepository.BulkInsertOrUpdate(channel);
            return channel.Id;
        }

        public FacebookStatisticDto FacebookStatistic(ChannelCrawl channel, ChannelFilter filter)
        {
            DateTime dateFrom = filter.CreatedTime.Min.Value;
            DateTime dateTo = filter.CreatedTime.Max.Value;
            double diff = DateUtil.DiffDate(dateFrom, dateTo);
            long follower = channel.ChannelRecords.Last().TotalFollower;
            var groupPost = channel.PostCrawls
                .Select(x => new
                {
                    x.Pid,
                    x.CreatedTime,
                    Reactions = x.Reactions.Select(r => new
                    {
                        r.ReactionTypeId,
                        r.Count,
                        r.ReactionType.Name
                    })
                }).ToList()
                .GroupBy(x => x.CreatedTime.Value.Date).ToList();

            var groupByDate = channel.PostCrawls
                .Select(x => new
                {
                    x.Pid,
                    x.CreatedTime,
                    Reactions = x.Reactions.Select(r => new
                    {
                        r.ReactionTypeId,
                        r.Count,
                        r.ReactionType.Name
                    })
                }).ToList()
                .GroupBy(x => x.CreatedTime.Value.Date.DayOfWeek).ToList();
            var groupType = channel.PostCrawls
                .Select(x => new
                {
                    x.Pid,
                    x.PostType,
                    x.CreatedTime,
                    
                }).ToList()
                .GroupBy(x => x.PostType).ToList();
            List<FacebookStatisticField> statistics = new List<FacebookStatisticField>();
            foreach (var post in groupPost)
            {
                FacebookStatisticField field = new FacebookStatisticField() { Date = post.Key };
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
                field.AverageEngagementERPost = Math.Round((float)(field.AverageEngagementRate) / field.TotalPost, 4);
                field.AverageDailyEngagementRate = Math.Round((float)(field.TotalReaction) / follower * 100, 4);
                field.AverageDailyEngagementReaction = Math.Round((float)(field.TotalReaction + field.TotalComment) / follower * 100, 4);


                statistics.Add(field);


            }
            foreach (var post in groupByDate)
            {
                FacebookStatisticField field = new FacebookStatisticField() { DateOfWeek = post.Key };
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
                        {-
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
                field.AverageEngagementERPostInDayOfWeek = Math.Round((float)(field.TotalReaction + field.TotalComment + field.TotalShare) / follower * 100, 4);
                field.AverageEngagementPostInDayOfWeek = Math.Round((float)(field.AverageEngagementRate) / field.TotalPost, 4);

                statistics.Add(field);

            }
           foreach (var post in groupType)
            {
                FacebookStatisticField field = new FacebookStatisticField() { DateType = post.Key };
                if (post.Key.Equals("added_photos"))
                {
                    field.TotalPostTypePhoto = post.LongCount();
                }
                if (post.Key.Equals("added_video"))
                {
                    field.TotalPostTypeStory = post.LongCount();
                }
                if (post.Key.Equals("shared_story"))
                {
                    field.TotalPostTypeVideo = post.LongCount();
                }
                field.TotalPost += 1;
                field.AverageEngagementRatePostType = Math.Round((float)(field.TotalPostTypePhoto + field.TotalPostTypeStory + field.TotalPostTypeVideo) / follower * 100, 4);
                statistics.Add(field);

            }

            var result = MapperConfig.GetMapper().Map<FacebookStatisticDto>(channel);
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

        public YoutubeStatisticDto YoutubeStatistic(ChannelCrawl channel, ChannelFilter filter)
        {
            DateTime dateFrom = filter.CreatedTime.Min.Value;
            DateTime dateTo = filter.CreatedTime.Max.Value;
            double diff = DateUtil.DiffDate(dateFrom, dateTo);
            long follower = channel.ChannelRecords.Last().TotalFollower;
            var groupPost = channel.PostCrawls
                .Select(x => new
                {
                    x.Pid,
                    x.CreatedTime,
                    Reactions = x.Reactions.Select(r => new
                    {
                        r.ReactionTypeId,
                        r.Count,
                        r.ReactionType.Name
                    })
                }).ToList()
                .GroupBy(x => x.CreatedTime.Value.Date).ToList();
            var groupByDate = channel.PostCrawls
                .Select(x => new
                {
                    x.Pid,
                    x.CreatedTime,
                    Reactions = x.Reactions.Select(r => new
                    {
                        r.ReactionTypeId,
                        r.Count,
                        r.ReactionType.Name
                    })
                }).ToList()
                .GroupBy(x => x.CreatedTime.Value.Date.DayOfWeek).ToList();


            List<YoutubeStatisticField> statistics = new List<YoutubeStatisticField>();
            foreach (var post in groupPost)
            {
                YoutubeStatisticField field = new YoutubeStatisticField() { Date = post.Key.Date };
                foreach (var item in post)
                {
                    foreach (var reaction in item.Reactions)
                    {

                        if (reaction.Name.Equals("reactionLike"))
                        {
                            field.TotalLike += reaction.Count;
                        }
                        if (reaction.Name.Equals("reactionView"))
                        {
                            field.TotalView += reaction.Count;
                        }
                        if (reaction.Name.Equals("reactionComment"))
                        {
                            field.TotalComment += reaction.Count;
                        }
                       

                    }
                    field.TotalPost += 1;
                }
                field.AverageEngagementRate = Math.Round((float)(field.TotalLike + field.TotalComment + field.TotalView) / follower * 100, 4);
                field.AverageEngagementViewER = Math.Round((float)(field.TotalLike) / field.TotalView * 100, 4);
                field.AverageEngagementView = Math.Round((float)(field.AverageEngagementViewER) / field.TotalPost, 4);
                statistics.Add(field);


            }
            foreach (var post in groupByDate)
            {
                YoutubeStatisticField field = new YoutubeStatisticField() { DateOfWeek = post.Key };
                foreach (var item in post)
                {
                    foreach (var reaction in item.Reactions)
                    {


                        if (reaction.Name.Equals("reactionLike"))
                        {
                            field.TotalLike += reaction.Count;
                        }
                        if (reaction.Name.Equals("reactionView"))
                        {
                            field.TotalView += reaction.Count;
                        }
                        if (reaction.Name.Equals("reactionComment"))
                        {
                            field.TotalComment += reaction.Count;
                        }

                    }
                    field.TotalPost += 1;
                }
                field.AverageEngagementERPostInDayOfWeek = Math.Round((float)(field.TotalLike + field.TotalComment + field.TotalView) / follower * 100, 4);
                field.AverageEngagementPostInDayOfWeek = Math.Round((float)(field.AverageEngagementRate) / field.TotalPost, 4);

                statistics.Add(field);

            }
            var result = MapperConfig.GetMapper().Map<YoutubeStatisticDto>(channel);
            result.StatisticFields = statistics;
            double difDate = DateUtil.DiffDate(result.CreatedTime, DateTime.Now);
            double difMonth = DateUtil.DiffMonth(result.CreatedTime, DateTime.Now);
            double difWeak = DateUtil.DiffWeek(result.CreatedTime, DateTime.Now);
            ChannelRecordDto recordDto = result.ChannelRecords.Last();
            long? totalRecord = recordDto.TotalLike + recordDto.TotalShare + recordDto.TotalComment;
            result.AveragePostInDay = Math.Round(recordDto.TotalPost / difDate, 2);
            result.AveragePostInMonth = Math.Round(recordDto.TotalPost / difMonth, 2);
            result.AveragePostInWeek = Math.Round(recordDto.TotalPost / difWeak, 2);
            
            result.AverageEngagementLikeInPost = (double)(recordDto.TotalLike / recordDto.TotalPost);
            result.AverageEngagementCommentRateInPost = (double)(recordDto.TotalComment / recordDto.TotalPost);
            result.AverageEngagementViewInPost = (double)(recordDto.TotalView / recordDto.TotalPost);
            return result;
        } 
        
        
        public TiktokStatisticDto TiktokStatistic(ChannelCrawl channel, ChannelFilter filter)
        {
            DateTime dateFrom = filter.CreatedTime.Min.Value;
            DateTime dateTo = filter.CreatedTime.Max.Value;
            double diff = DateUtil.DiffDate(dateFrom, dateTo);
            long follower = channel.ChannelRecords.Last().TotalFollower;
            var groupPost = channel.PostCrawls
                .Select(x => new
                {
                    x.Pid,
                    x.CreatedTime,
                    Reactions = x.Reactions.Select(r => new
                    {
                        r.ReactionTypeId,
                        r.Count,
                        r.ReactionType.Name
                    })
                }).ToList()
                .GroupBy(x => x.CreatedTime.Value.Date).ToList();
            var groupByDate = channel.PostCrawls
            .Select(x => new
            {
                x.Pid,
                x.CreatedTime,
                Reactions = x.Reactions.Select(r => new
                {
                    r.ReactionTypeId,
                    r.Count,
                    r.ReactionType.Name
                })
            }).ToList()
            .GroupBy(x => x.CreatedTime.Value.Date.DayOfWeek).ToList();

            List<TiktokStatisticField> statistics = new List<TiktokStatisticField>();
            foreach (var post in groupPost)
            {
                TiktokStatisticField field = new TiktokStatisticField() { Date = post.Key.Date };
                foreach (var item in post)
                {
                    foreach (var reaction in item.Reactions)
                    {

                        if (reaction.Name.Equals("diggCount"))
                        {
                            field.TotalLike += reaction.Count;
                        }
                        if (reaction.Name.Equals("playCount"))
                        {
                            field.TotalView += reaction.Count;
                        }
                        if (reaction.Name.Equals("commentCount"))
                        {
                            field.TotalComment += reaction.Count;
                        } 
                        if (reaction.Name.Equals("shareCount"))
                        {
                            field.TotalShare += reaction.Count;
                        }
                       

                    }
                    field.TotalPost += 1;
                }
                field.AverageEngagementRate = Math.Round((float)(field.TotalLike + field.TotalComment + field.TotalView + field.TotalShare) / follower * 100, 4);
                field.AverageEngagementViewER = Math.Round((float)(field.TotalLike) / field.TotalView * 100, 4);
                field.AverageEngagementView = Math.Round((float)(field.AverageEngagementViewER) / field.TotalPost, 4);

                statistics.Add(field);


            }
            foreach (var post in groupByDate)
            {
                TiktokStatisticField field = new TiktokStatisticField() { DateOfWeek = post.Key };
                foreach (var item in post)
                {
                    foreach (var reaction in item.Reactions)
                    {


                        if(reaction.Name.Equals("diggCount"))
                        {
                            field.TotalLike += reaction.Count;
                        }
                        if (reaction.Name.Equals("playCount"))
                        {
                            field.TotalView += reaction.Count;
                        }
                        if (reaction.Name.Equals("commentCount"))
                        {
                            field.TotalComment += reaction.Count;
                        }
                        if (reaction.Name.Equals("shareCount"))
                        {
                            field.TotalShare += reaction.Count;
                        }

                    }
                    field.TotalPost += 1;
                }
                field.AverageEngagementERPostInDayOfWeek = Math.Round((float)(field.TotalLike + field.TotalComment + field.TotalView + field.TotalShare) / follower * 100, 4);
                field.AverageEngagementPostInDayOfWeek = Math.Round((float)(field.AverageEngagementRate) / field.TotalPost, 4);

                statistics.Add(field);

            }
            var result = MapperConfig.GetMapper().Map<TiktokStatisticDto>(channel);
            result.StatisticFields = statistics;
            double difDate = DateUtil.DiffDate(result.CreatedTime, DateTime.Now);
            double difMonth = DateUtil.DiffMonth(result.CreatedTime, DateTime.Now);
            double difWeak = DateUtil.DiffWeek(result.CreatedTime, DateTime.Now);
            ChannelRecordDto recordDto = result.ChannelRecords.Last();
            long? totalRecord = recordDto.TotalLike + recordDto.TotalShare + recordDto.TotalComment + recordDto.TotalView;
            result.AveragePostInDay = Math.Round(recordDto.TotalPost / difDate, 2);
            result.AveragePostInMonth = Math.Round(recordDto.TotalPost / difMonth, 2);
            result.AveragePostInWeek = Math.Round(recordDto.TotalPost / difWeak, 2);
            
            result.AverageEngagementLikeInPost = (double)(recordDto.TotalLike / recordDto.TotalPost);
            result.AverageEngagementCommentRateInPost = (double)(recordDto.TotalComment / recordDto.TotalPost);
            result.AverageEngagementViewInPost = (double)(recordDto.TotalView / recordDto.TotalPost);
            result.AverageEngagementShareInPost = (double)(recordDto.TotalShare / recordDto.TotalPost);
            return result;
        }

        public async Task<int> FindChannelByPlatformAndUserId(string url)
        {
            var result = RegexPlatformAndUser(url);
            if (result.Item1 == null)
            {
                throw new Exception("Invalid url");
            }
            
            int platformId = (int)Enum.Parse<PlatFormEnum>(result.Item1);
            var channel = await _channelCrawlRepository.Get(x => x.PlatformId == platformId && (x.Cid == result.Item2 || x.Username == result.Item2));
            if (channel == null)
            {
                
                return 0;
            }
            else
            {
                return channel.Id;
            }
        }
    }
}
