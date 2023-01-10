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
using DataAccess.Enum;
using Business.Constants;
using DataAccess.Models.ChannelCrawlModel.FacebookStatistic;
using DataAccess.Models.ChannelCrawlModel.YoutubeStatistic;
using DataAccess.Models.ChannelCrawlModel.TiktokStatistic;
using DataAccess.Models.Pagination;
using DataAccess.Models.ChannelCrawlModel.CompareModel;
using DataAccess.Repository.UserTypeRepo;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Cryptography;

namespace Business.Service.ChannelCrawlService
{
    public class ChannelCrawlService : BaseService, IChannelCrawlService
    {
        private readonly IChannelCrawlRepository _channelCrawlRepository;
        private readonly IDistributedCache _cache;
        private string ClassName = typeof(ChannelCrawl).Name;
        public ChannelCrawlService(IHttpContextAccessor httpContextAccessor
            , IUserRepository userRepository
            , IUserTypeRepository userTypeRepository
            , IChannelCrawlRepository channelCrawlRepository
            , IDistributedCache cache) :
            base(httpContextAccessor, userRepository, userTypeRepository)
        {
            _channelCrawlRepository = channelCrawlRepository;
            _cache = cache;
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

        public FacebookStatisticDto FacebookStatistic(ChannelStatistic channel, ChannelFilter filter)
        {
            DateTime dateFrom = filter.CreatedTime.Min.Value;
            DateTime dateTo = filter.CreatedTime.Max.Value;
            double diff = DateUtil.DiffDate(dateFrom, dateTo);
            long follower = channel.ChannelRecords.Last().TotalFollower;
            var result = MapperConfig.GetMapper().Map<FacebookStatisticDto>(channel);
            if (channel.PostCrawls.Count > 0)
            {
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
                    }).ToList().OrderBy(x => x.CreatedTime.Value.Date.DayOfWeek)
                    .GroupBy(x => x.CreatedTime.Value.Date.DayOfWeek);
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
                List<FacebookStatisticFieldInWeek> statisticsInWeek = new List<FacebookStatisticFieldInWeek>();
                foreach (var post in groupByDate)
                {
                    FacebookStatisticFieldInWeek field = new FacebookStatisticFieldInWeek() { DateOfWeek = post.Key.ToString() };
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
                    field.AverageEngagementRateDayOfWeek = Math.Round((float)(field.TotalReaction + field.TotalComment + field.TotalShare) / follower * 100, 4);
                    field.AverageEngagementERPostInDayOfWeek = Math.Round((float)(field.TotalReaction + field.TotalComment + field.TotalShare) / follower * 100, 4);
                    field.AverageEngagementPostInDayOfWeek = Math.Round((float)(field.AverageEngagementRateDayOfWeek) / field.TotalPost, 4);

                    statisticsInWeek.Add(field);

                }

                List<FacebookStatisticFieldPostType> statisticsPostType = new List<FacebookStatisticFieldPostType>();
                foreach (var post in groupType)
                {

                    FacebookStatisticFieldPostType field = new FacebookStatisticFieldPostType() { PostType = post.Key };

                    field.Count = post.LongCount();
                    statisticsPostType.Add(field);

                }
                result.StatisticFields = statistics;
                result.StatisticFieldsInWeek = statisticsInWeek;
                result.StatisticFieldsPostType = statisticsPostType;
            }

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

        public YoutubeStatisticDto YoutubeStatistic(ChannelStatistic channel, ChannelFilter filter)
        {
            DateTime dateFrom = filter.CreatedTime.Min.Value;
            DateTime dateTo = filter.CreatedTime.Max.Value;
            double diff = DateUtil.DiffDate(dateFrom, dateTo);
            long follower = channel.ChannelRecords.Last().TotalFollower;
            var result = MapperConfig.GetMapper().Map<YoutubeStatisticDto>(channel);
            if (channel.PostCrawls.Count > 0)
            {
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
                    }).ToList().OrderBy(x => x.CreatedTime.Value.Date.DayOfWeek)
                    .GroupBy(x => x.CreatedTime.Value.Date.DayOfWeek).ToList();

                var groupType = channel.PostCrawls
                    .Select(x => new
                    {
                        x.Pid,
                        x.PostType,
                        x.CreatedTime,

                    }).ToList()
                    .GroupBy(x => x.PostType).ToList();
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
                List<YoutubeStatisticFieldInWeek> statisticsInWeek = new List<YoutubeStatisticFieldInWeek>();
                foreach (var post in groupByDate)
                {
                    YoutubeStatisticFieldInWeek field = new YoutubeStatisticFieldInWeek() { DateOfWeek = post.Key.ToString() };
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
                    field.AverageEngagementRateDayOfWeek = Math.Round((float)(field.TotalLike + field.TotalComment + field.TotalView) / follower * 100, 4);
                    field.AverageEngagementERPostInDayOfWeek = Math.Round((float)(field.TotalLike + field.TotalComment + field.TotalView) / follower * 100, 4);
                    field.AverageEngagementPostInDayOfWeek = Math.Round((float)(field.AverageEngagementRateDayOfWeek) / field.TotalPost, 4);

                    statisticsInWeek.Add(field);

                }

                List<YoutubeStatisticFieldPostType> statisticsPostType = new List<YoutubeStatisticFieldPostType>();
                foreach (var post in groupType)
                {

                    YoutubeStatisticFieldPostType field = new YoutubeStatisticFieldPostType() { PostType = post.Key };
                    field.Count = post.LongCount();
                    statisticsPostType.Add(field);
                }
                result.StatisticFields = statistics;
                result.StatisticFieldsInWeek = statisticsInWeek;
                result.StatisticFieldsPostType = statisticsPostType;
            }

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


        public TiktokStatisticDto TiktokStatistic(ChannelStatistic channel, ChannelFilter filter)
        {

            DateTime dateFrom = filter.CreatedTime.Min.Value;
            DateTime dateTo = filter.CreatedTime.Max.Value;
            double diff = DateUtil.DiffDate(dateFrom, dateTo);
            long follower = channel.ChannelRecords.Last().TotalFollower;
            var result = MapperConfig.GetMapper().Map<TiktokStatisticDto>(channel);
            if (channel.PostCrawls.Count > 0)
            {
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
                }).ToList().OrderBy(x => x.CreatedTime.Value.Date.DayOfWeek)
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
                List<TiktokStatisticFieldInWeek> statisticsInWeek = new List<TiktokStatisticFieldInWeek>();
                foreach (var post in groupByDate)
                {
                    TiktokStatisticFieldInWeek field = new TiktokStatisticFieldInWeek() { DateOfWeek = post.Key.ToString() };
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
                    field.AverageEngagementRateInWeek = Math.Round((float)(field.TotalLike + field.TotalComment + field.TotalView + field.TotalShare) / follower * 100, 4);
                    field.AverageEngagementERPostInDayOfWeek = Math.Round((float)(field.TotalLike + field.TotalComment + field.TotalView + field.TotalShare) / follower * 100, 4);
                    field.AverageEngagementPostInDayOfWeek = Math.Round((float)(field.AverageEngagementRateInWeek) / field.TotalPost, 4);

                    statisticsInWeek.Add(field);

                }
                result.StatisticFields = statistics;
                result.StatisticFieldsInWeek = statisticsInWeek;
            }
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

        public async Task<string> FindChannelByPlatformAndUserId(string url)
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

                return "";
            }
            else
            {
                return channel.Username;
            }
        }

        public async Task<PaginationList<ChannelCrawlDto>> SearchAsync(ChannelSearchFilter paging)
        {
            var result = await _channelCrawlRepository.SearchAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<ChannelCrawlDto>>(result.Items);
            return new PaginationList<ChannelCrawlDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalItem = result.TotalItem,
                TotalPage = result.TotalPage
            };
        }

        public async Task<int> Update(ChannelCrawlDto dto)
        {
            var check = await _channelCrawlRepository.Get(x => x.Id == dto.Id);
            if (check == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var channel = MapperConfig.GetMapper().Map<ChannelCrawl>(dto);

            await _channelCrawlRepository.ValidateChannelAsync(channel);

            await _channelCrawlRepository.Update(channel);
            return channel.Id;
        }

        public async Task<object> CompareChannel(CompareDto dto)
        {
            ChannelFilter channelFilter = new ChannelFilter() { Platform = dto.Platform, CreatedTime = dto.CreatedTime };
            List<string> userIds = new List<string>() { dto.UserIdOne, dto.UserIdTwo };
            var result = await _channelCrawlRepository.FilterChannels(userIds, channelFilter);
            if (result.Count == 0)
            {
                throw new Exception("No channel found on data. Please request view data first");
            }
            else if (result.Count == 1)
            {
                throw new Exception("Only " + result[0].Username + " channel found. Please request orther first to compare data");
            }
            else
            {
                int platform = dto.Platform;
                var data = new List<object>();

                var fields = new Object();
                switch (platform)
                {

                    case (int)EnumConst.PlatFormEnum.FACEBOOK:

                        data = new List<object>() { FacebookStatistic(result[0], channelFilter), FacebookStatistic(result[1], channelFilter) };

                        fields = CreateCompareStatisticField(((FacebookStatisticDto)data[0]).StatisticFields, ((FacebookStatisticDto)data[1]).StatisticFields, dto);
                        break;
                    case (int)EnumConst.PlatFormEnum.YOUTUBE:
                        data = new List<object>() { YoutubeStatistic(result[0], channelFilter), YoutubeStatistic(result[1], channelFilter) };
                        fields = CreateCompareStatisticField(((YoutubeStatisticDto)data[0]).StatisticFields, ((YoutubeStatisticDto)data[1]).StatisticFields, dto);

                        break;
                    case (int)EnumConst.PlatFormEnum.TIKTOK:
                        data = new List<object>() { TiktokStatistic(result[0], channelFilter), TiktokStatistic(result[1], channelFilter) };
                        fields = CreateCompareStatisticField(((TiktokStatisticDto)data[0]).StatisticFields, ((TiktokStatisticDto)data[1]).StatisticFields, dto);

                        break;
                    default: return null;

                }
                var informations = MapperConfig.GetMapper().Map<List<ChannelStatisticDto>>(data);
                return new
                {
                    left = informations[0],
                    right = informations[1]
                    ,
                    ChannelRecord = CreateChannelRecordCompare(informations[0], informations[1])
                    ,
                    StatisticFiled = fields
                };
            }

        }

        public async Task<object> StatisticTopPost(int id)
        {
            return await _channelCrawlRepository.FilterTopPost(id);
        }

        private List<Dictionary<string, object>> CreateChannelRecordCompare(ChannelStatisticDto userLeft, ChannelStatisticDto userRight)
        {
            var fieldValues = typeof(InsertChannelRecordDto).GetProperties().Select(f => f.Name).ToList();
            var usernameLeft = userLeft.Username;
            var usernameRight = userRight.Username;
            var lastRecordLeft = userLeft.ChannelRecords.Last();
            var lastRecordRight = userRight.ChannelRecords.Last();
            var records = new List<Dictionary<string, object>>();

            foreach (var field in fieldValues)
            {
                if (!field.Equals("Status") && !field.Equals("ChannelId") && !field.Equals("CreatedDate") && !field.Equals("UpdateDate"))
                {
                    var dataLeft = lastRecordLeft.GetType().GetProperty(field).GetValue(lastRecordLeft, null);
                    var dataRight = lastRecordRight.GetType().GetProperty(field).GetValue(lastRecordRight, null);
                    if (dataLeft != null || dataRight != null)
                    {

                        Dictionary<string, object> record = new Dictionary<string, object>();
                        record.Add("Name", field);
                        record.Add("UserLeft", dataLeft);
                        record.Add("UserRight", dataRight);
                        records.Add(record);

                    }
                }
            }
            return records;
        }

        private List<Dictionary<string, object>> CreateCompareStatisticField<T>(List<T> userLeft, List<T> userRight, CompareDto dto) where T : BaseFieldStatistic
        {
            var fieldValues = new List<string>();

            if (userLeft is List<FacebookStatisticField>)
            {
                fieldValues = typeof(FacebookStatisticField).GetProperties().Select(f => f.Name).ToList();

            }
            else if (userLeft is List<YoutubeStatisticField>)
            {
                fieldValues = typeof(YoutubeStatisticField).GetProperties().Select(f => f.Name).ToList();

            }
            else if (userLeft is List<TiktokStatisticField>)
            {
                fieldValues = typeof(TiktokStatisticField).GetProperties().Select(f => f.Name).ToList();
            }

            var records = new List<Dictionary<string, object>>();
            var from = dto.CreatedTime.Min.GetValueOrDefault();
            var to = dto.CreatedTime.Max.GetValueOrDefault();
            var fieldsLeft = userLeft.ToDictionary(x => x.Date, x => x);
            var fieldsRight = userRight.ToDictionary(x => x.Date, x => x);
            for (var day = from.Date; day < to.Date; day = day.AddDays(1))
            {
                var field1 = fieldsLeft.ContainsKey(day) ? fieldsLeft[day] : null;
                var field2 = fieldsRight.ContainsKey(day) ? fieldsRight[day] : null;
                Dictionary<string, object> statisticField = new Dictionary<string, object>();
                bool flag = true;
                foreach (var field in fieldValues)
                {
                    if (!field.Equals("Date"))
                    {
                        if (field1 != null)
                        {
                            statisticField.Add(field + "UserLeft", field1.GetType().GetProperty(field).GetValue(field1, null));
                            flag = false;
                        }

                        if (field2 != null)
                        {
                            statisticField.Add(field + "UserRight", field2.GetType().GetProperty(field).GetValue(field2, null));
                            flag = false;
                        }
                    }

                }
                if (!flag)
                {
                    statisticField.Add("Date", day);
                    records.Add(statisticField);
                }
            }

            return records;
        }

        public async void UpdateCache()
        {
            int uid = GetCurrentUserId();
            var result = await _cache.GetAsync(uid.ToString());

            if (result == null)
            {
                var userType = await GetCurrentUserType();
                var data = UpdateFeatureQuantity(userType.Feature, "DAILYSEARCH");
                if (data != null)
                {
                    var options = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromDays(1));
                    await _cache.SetAsync(uid.ToString(), SerializationUtil.ToByteArray(data), options);
                }
            }
            var newCacheJson = UpdateFeatureQuantity(SerializationUtil.FromByteArray<string>(result), "DAILYSEARCH");
            if (newCacheJson != null)
            {
                await _cache.SetAsync(uid.ToString(), SerializationUtil.ToByteArray(newCacheJson));
            }

        }

        private string UpdateFeatureQuantity(string featureJson, string field)
        {
            var feature = SerializationUtil.FromString(featureJson);
            if (feature.ContainsKey(field))
            {
                feature[field].Quota -= 1;
                return SerializationUtil.ToString(feature);
            }
            return null;
        }

        public async void UpdateChannelRequest()
        {
            int uid = GetCurrentUserId();
            var userType = await GetCurrentUserType();
            var data = UpdateFeatureQuantity(userType.Feature, "MONTHREQUEST");
            if (data != null)
            {
                var cache = await _cache.GetAsync(uid.ToString());
                if (cache == null)
                {
                    var options = new DistributedCacheEntryOptions()
           .SetSlidingExpiration(TimeSpan.FromDays(1));
                    await _cache.SetAsync(uid.ToString(), SerializationUtil.ToByteArray(data), options);
                }
                else
                {
                    await _cache.SetAsync(uid.ToString(), SerializationUtil.ToByteArray(data));
                }
                
            }
        }
    }
}
