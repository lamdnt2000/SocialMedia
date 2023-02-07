using API.Utils;
using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Enum;
using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.Pagination;
using DataAccess.Models.ReactionModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Z.BulkOperations;

namespace Business.Repository.ChannelCrawlRepo
{
    public class ChannelCrawlRepository : GenericRepository<ChannelCrawl>, IChannelCrawlRepository
    {
        public ChannelCrawlRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<bool> BulkInsertOrUpdate(ChannelCrawl entity)
        {
            List<ChannelCrawl> list = new List<ChannelCrawl>() { entity };
            ICollection<PostCrawl> postCrawls = entity.PostCrawls;
            if (entity.Id != 0)
            {
                var record = await context.ChannelRecords.OrderBy(x => x.CreatedDate).LastOrDefaultAsync(x => x.ChannelId == entity.Id);
                DateTime now = DateTime.Now;
                double diffDate = DateUtil.DiffDate(record.CreatedDate.Value.Date.Date, now.Date);
                if (diffDate == 0)
                {
                    entity.ChannelRecords.FirstOrDefault(x => x.ChannelId == entity.Id).Id = record.Id;
                }
            }

            await context.BulkMergeAsync(list, options =>
            {
                options.IncludeGraph = true;
                options.InsertIfNotExists = true;
                options.UseRowsAffected = true;
                options.IncludeGraphOperationBuilder = operation =>
                {
                    if (operation is BulkOperation<ChannelCrawl> bulkChannelCraw)
                    {
                        bulkChannelCraw.ColumnPrimaryKeyExpression = expression => new { expression.Id };

                        bulkChannelCraw.IgnoreOnMergeUpdateExpression = e => new { e.CreatedDate };
                        bulkChannelCraw.IgnoreOnMergeInsertExpression = c => new { c.UpdateDate };
                    }
                    if (operation is BulkOperation<ChannelRecord> bulkRecord)
                    {
                        bulkRecord.ColumnPrimaryKeyExpression = expression => new { expression.Id };

                        bulkRecord.IgnoreOnMergeUpdateExpression = e => new { e.CreatedDate };
                        bulkRecord.IgnoreOnMergeInsertExpression = c => new { c.UpdateDate };
                    }
                    if (operation is BulkOperation<PostCrawl> bulkPostCrawl)
                    {
                        bulkPostCrawl.ColumnPrimaryKeyExpression = expression => new { expression.Pid };
                        bulkPostCrawl.IgnoreOnSynchronizeMatchedAndConditionExpression = e => new
                        {
                            e.Status,
                            e.Body,
                            e.PostType,
                            e.Pid,
                            e.Description,
                            e.ChannelId,
                            e.Title,
                            e.CreatedTime
                        };
                        bulkPostCrawl.IgnoreOnMergeUpdateExpression = e => new { e.CreatedDate, e.Pid, e.Status };
                        bulkPostCrawl.IgnoreOnMergeInsertExpression = c => new { c.UpdateDate };
                    }
                    else if (operation is BulkOperation<Reaction> bulkReaction)
                    {
                        bulkReaction.ColumnPrimaryKeyExpression = expression => new { expression.ReactionTypeId, expression.PostId };
                        bulkReaction.IgnoreOnSynchronizeMatchedAndConditionExpression = e => new
                        {
                            e.Status,
                            e.Count,
                            e.PostId,
                            e.ReactionTypeId
                        };
                        bulkReaction.IgnoreOnMergeUpdateExpression = e => new { e.CreatedDate, e.ReactionType, e.PostId, e.Status };
                        bulkReaction.IgnoreOnMergeInsertExpression = c => new { c.UpdateDate };

                    }
                };

            });

            return true;
        }

        public async Task<ChannelStatistic> FilterChannel(ChannelFilter filter)
        {
            
            DateTime dateFrom = filter.CreatedTime.Min.Value;
            DateTime dateTo = filter.CreatedTime.Max.Value.AddDays(1);
            var channel = await context.ChannelCrawls.Where(c => (filter.Platform > 0 ? c.PlatformId == filter.Platform : true) && (c.Username == filter.Username || c.Cid == filter.Username))
                .Select(c => new ChannelStatistic()


                {
                    Cid = c.Cid,
                    Id = c.Id,
                    Location = c.Location.Name,
                    IsVerify = c.IsVerify,
                    Name = c.Name,
                    Organization = c.Organization.Name,
                    Platform = c.Platform.Name,
                    PostCrawls = c.PostCrawls.Where(p => p.CreatedTime >= dateFrom && p.CreatedTime < dateTo).Select(p =>
                        new PostCrawl()
                        {
                            Pid = p.Pid,
                            CreatedTime = p.CreatedTime,
                            PostType = p.PostType,
                            Reactions = p.Reactions.Select(r => new Reaction()
                            {
                                Count = r.Count,
                                ReactionType = r.ReactionType,
                            }).ToList()

                        }).ToList(),
                    Status = c.Status,
                    UpdateDate = c.UpdateDate.Value,
                    CreatedDate = c.CreatedDate.Value,
                    CreatedTime = c.CreatedTime.Value,
                    Bio = c.Bio,
                    AvatarUrl = c.AvatarUrl,
                    BannerUrl = c.BannerUrl,
                    Url = c.Url,
                    PlatformId = c.PlatformId,
                    Username = c.Username,
                    ChannelRecords = c.ChannelRecords,
                    Categories = c.ChannelCategories.Select(x => x.Category.Name).ToList()
                })
                .FirstOrDefaultAsync();


            return channel;


        }
        public async Task<List<ChannelStatistic>> FilterChannels(List<string> userIds, ChannelFilter filter)
        {
            var checkValid = await context.ChannelCrawls.Where(c => c.Username == userIds[0] || c.Username == userIds[1]).CountAsync();
            if (checkValid < 2)
            {
                return null;
            }
            DateTime dateFrom = filter.CreatedTime.Min.Value;
            DateTime dateTo = filter.CreatedTime.Max.Value.AddDays(1);
            List<ChannelStatistic> channels = new List<ChannelStatistic>();

            foreach (string userId in userIds)
            {
                var channel = await context.ChannelCrawls.AsNoTrackingWithIdentityResolution()
                .Where(c => c.PlatformId == filter.Platform && c.Username == userId)
                .Select(c => new ChannelStatistic()

                {
                    Cid = c.Cid,
                    Id = c.Id,
                    Location = c.Location.Name,
                    IsVerify = c.IsVerify,
                    Name = c.Name,
                    Organization = c.Organization.Name,
                    Platform = c.Platform.Name,
                    PostCrawls = c.PostCrawls.Where(p => p.CreatedTime >= dateFrom && p.CreatedTime < dateTo).Select(p =>
                        new PostCrawl()
                        {
                            CreatedTime = p.CreatedTime,
                            Reactions = p.Reactions.Select(r => new Reaction()
                            {
                                Count = r.Count,
                                ReactionTypeId = r.ReactionTypeId,
                                ReactionType = r.ReactionType,
                            }).ToList()

                        }).ToList(),
                    Status = c.Status,
                    UpdateDate = c.UpdateDate.Value,
                    CreatedTime = c.CreatedTime.Value,
                    Bio = c.Bio,
                    AvatarUrl = c.AvatarUrl,
                    BannerUrl = c.BannerUrl,
                    Url = c.Url,
                    PlatformId = c.PlatformId,
                    Username = c.Username,
                    ChannelRecords = c.ChannelRecords,
                    Categories = c.ChannelCategories.Select(x => x.Category.Name).ToList()
                }).FirstOrDefaultAsync();
                channels.Add(channel);
            }
            return channels;

            /*return context.ChannelCrawls.AsNoTrackingWithIdentityResolution()
                .Where(c => (c.PlatformId == filter.Platform && c.Username == userIds[0]) || (c.PlatformId == filter.Platform && c.Username == userIds[1]))
                
                .Select(c => new ChannelStatistic()

                {
                    Cid = c.Cid,
                    Id = c.Id,
                    Location = c.Location.Name,
                    IsVerify = c.IsVerify,
                    Name = c.Name,
                    Organization = c.Organization.Name,
                    Platform = c.Platform.Name,
                    PostCrawls = c.PostCrawls.Where(p => p.CreatedTime >= dateFrom && p.CreatedTime < dateTo).Select(p =>
                        new PostCrawl()
                        {
                            CreatedTime = p.CreatedTime,
                            Reactions = p.Reactions.Select(r => new Reaction()
                            {
                                Count = r.Count,
                                ReactionTypeId = r.ReactionTypeId,
                                ReactionType = r.ReactionType,
                            }).ToList()

                        }).ToList(),
                    Status = c.Status,
                    UpdateDate = c.UpdateDate.Value,
                    CreatedTime = c.CreatedTime.Value,
                    Bio = c.Bio,
                    AvatarUrl = c.AvatarUrl,
                    BannerUrl = c.BannerUrl,
                    Url = c.Url,
                    PlatformId = c.PlatformId,
                    Username = c.Username,
                    ChannelRecords = c.ChannelRecords,
                    Categories = c.ChannelCategories.Select(x => x.Category.Name).ToList()
                }).AsEnumerable().ToList() ;*/


        }

        public async Task<bool> ValidateChannelAsync(ChannelCrawl entity)
        {
            if (!context.Organizations.Any(x => x.Id == entity.OrganizationId))
            {
                throw new Exception("Organization not exist!");
            }
            if (entity.BrandId != 0)
            {
                if (!context.Brands.Any(x => x.Id == entity.BrandId))
                {
                    throw new Exception("Brand not exist!");
                }
            }
            if (!context.Platforms.Any(x => x.Id == entity.PlatformId))
            {
                throw new Exception("Platform not exist!");
            }
            if (entity.ChannelCategories != null)
            {
                foreach (ChannelCategory c in entity.ChannelCategories)
                {
                    if (!context.Categories.Any(x => x.Id == c.CategoryId))
                    {
                        throw new Exception("Category not exist!");
                    }
                }
            }
            if (!context.Locations.Any(x => x.Id == entity.LocationId))
            {
                throw new Exception("Location not exist!");
            }
            if (entity.Id == 0)
            {
                if ((await context.ChannelCrawls.FirstOrDefaultAsync(x => x.PlatformId == entity.PlatformId && x.Cid == entity.Cid ) != null))
                {
                    throw new Exception("Duplicated channel");
                }
            }
    
            

            return true;
        }

        public async Task<PaginationList<ChannelCrawl>> SearchAsync(ChannelSearchFilter paging)
        {
            var totalItem = await context.ChannelCrawls.ApplyFilterWithoutPagination(paging).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.ChannelCrawls.ApplyFilter(paging).Include(c => c.Organization).
                Include(c => c.Platform).Include(c => c.ChannelCategories).ThenInclude(c => c.Category).ToList();
            return new PaginationList<ChannelCrawl>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = (int)totalPage,
                TotalItem = totalItem,
                Items = result
            };
        }

        public async Task<Dictionary<string, object>> FilterTopPostFacebookChannel(int id)
        {

            var topLike = await context.PostCrawls.Where(x => x.ChannelId == id)
                .Select(c => new
                {
                    Pid = c.Pid,
                    Body = c.Body,
                    CreatedDate = c.CreatedDate,
                    UpdateDate = c.UpdateDate,
                    Description = c.Description,
                    CreatedTime = c.CreatedTime,
                    PostType = c.PostType,
                    Status = c.Status,
                    Title = c.Title,

                    Reactions = c.Reactions
                    .Select(r => new TopPostReaction()
                    {
                        Count = r.Count,
                        ReactionName = r.ReactionType.Name
                    }).ToList()
                }
                ).OrderByDescending(c =>
                c.Reactions.Where(x => x.ReactionName == "totalLike")
                .Select(x => x.Count)
                .OrderByDescending(x => x)
                .FirstOrDefault())
                .Take(10).ToListAsync();
            var topComment = await context.PostCrawls.Where(x => x.ChannelId == id)
                .Select(c => new
                {
                    Pid = c.Pid,
                    Body = c.Body,
                    CreatedDate = c.CreatedDate,
                    UpdateDate = c.UpdateDate,
                    Description = c.Description,
                    CreatedTime = c.CreatedTime,
                    PostType = c.PostType,
                    Status = c.Status,
                    Title = c.Title,
                    Reactions = c.Reactions
                    .Select(r => new TopPostReaction()
                    {
                        Count = r.Count,
                        ReactionName = r.ReactionType.Name
                    }).ToList()
                }
                ).OrderByDescending(c =>
                c.Reactions.Where(x => x.ReactionName == "comment")
                .Select(x => x.Count)
                .OrderByDescending(x => x)
                .FirstOrDefault())
                .Take(10).ToListAsync();
            var topShare = await context.PostCrawls.Where(x => x.ChannelId == id)
                .Select(c => new
                {
                    Pid = c.Pid,
                    Body = c.Body,
                    CreatedDate = c.CreatedDate,
                    UpdateDate = c.UpdateDate,
                    Description = c.Description,
                    CreatedTime = c.CreatedTime,
                    PostType = c.PostType,
                    Status = c.Status,
                    Title = c.Title,

                    Reactions = c.Reactions

                    .Select(r => new TopPostReaction()
                    {
                        Count = r.Count,
                        ReactionName = r.ReactionType.Name
                    }).ToList()
                }
                ).OrderByDescending(c =>
                c.Reactions.Where(x => x.ReactionName == "share")
                .Select(x => x.Count)
                .OrderByDescending(x => x)
                .FirstOrDefault())
                .Take(10).ToListAsync();
            return new Dictionary<string, object>() {
                {"like", topLike },
                {"comment",topComment },
                {"share",topShare } };
        }

        public async Task<Dictionary<string, object>> FilterTopPostTiktokChannel(int id)
        {
            var topHeart = await context.PostCrawls.Where(x => x.ChannelId == id)
                .Select(c => new
                {
                    Pid = c.Pid,
                    Body = c.Body,
                    CreatedDate = c.CreatedDate,
                    UpdateDate = c.UpdateDate,
                    Description = c.Description,
                    CreatedTime = c.CreatedTime,
                    PostType = c.PostType,
                    Status = c.Status,
                    Title = c.Title,

                    Reactions = c.Reactions
                    .Select(r => new TopPostReaction()
                    {
                        
                        Count = r.Count,
                        ReactionName = r.ReactionType.Name
                    }).ToList()
                }
                ).OrderByDescending(c =>
                c.Reactions.Where(x => x.ReactionName == "diggCount")
                .Select(x => x.Count)
                .OrderByDescending(x => x)
                .FirstOrDefault())
                .Take(10).ToListAsync();
            var topView = await context.PostCrawls.Where(x => x.ChannelId == id)
                .Select(c => new
                {
                    Pid = c.Pid,
                    Body = c.Body,
                    CreatedDate = c.CreatedDate,
                    UpdateDate = c.UpdateDate,
                    Description = c.Description,
                    CreatedTime = c.CreatedTime,
                    PostType = c.PostType,
                    Status = c.Status,
                    Title = c.Title,

                    Reactions = c.Reactions
                    .Select(r => new TopPostReaction()
                    {
                        Count = r.Count,
                        ReactionName = r.ReactionType.Name
                    }).ToList()
                }
                ).OrderByDescending(c =>
                c.Reactions.Where(x => x.ReactionName == "playCount")
                .Select(x => x.Count)
                .OrderByDescending(x => x)
                .FirstOrDefault())
                .Take(10).ToListAsync();
            var topShare = await context.PostCrawls.Where(x => x.ChannelId == id)
                .Select(c => new
                {
                    Pid = c.Pid,
                    Body = c.Body,
                    CreatedDate = c.CreatedDate,
                    UpdateDate = c.UpdateDate,
                    Description = c.Description,
                    CreatedTime = c.CreatedTime,
                    PostType = c.PostType,
                    Status = c.Status,
                    Title = c.Title,

                    Reactions = c.Reactions

                    .Select(r => new TopPostReaction()
                    {
                        Count = r.Count,
                        ReactionName = r.ReactionType.Name
                    }).ToList()
                }
                ).OrderByDescending(c =>
                c.Reactions.Where(x => x.ReactionName == "shareCount")
                .Select(x => x.Count)
                .OrderByDescending(x => x)
                .FirstOrDefault())
                .Take(10).ToListAsync();
            var topComment = await context.PostCrawls.Where(x => x.ChannelId == id)
                .Select(c => new
                {
                    Pid = c.Pid,
                    Body = c.Body,
                    CreatedDate = c.CreatedDate,
                    UpdateDate = c.UpdateDate,
                    Description = c.Description,
                    CreatedTime = c.CreatedTime,
                    PostType = c.PostType,
                    Status = c.Status,
                    Title = c.Title,

                    Reactions = c.Reactions

                    .Select(r => new TopPostReaction()
                    {
                        Count = r.Count,
                        ReactionName = r.ReactionType.Name
                    }).ToList()
                }
                ).OrderByDescending(c =>
                c.Reactions.Where(x => x.ReactionName == "commentCount")
                .Select(x => x.Count)
                .OrderByDescending(x => x)
                .FirstOrDefault())
                .Take(10).ToListAsync();
            return new Dictionary<string, object>() {
                {"heart", topHeart },
                {"comment",topComment },
                {"share",topShare },
                {"view",topView } };
        }

        public async Task<Dictionary<string, object>> FilterTopPostYoutubeChannel(int id)
        {
            var topLike = await context.PostCrawls.Where(x => x.ChannelId == id)
                .Select(c => new
                {
                    Pid = c.Pid,
                    Body = c.Body,
                    CreatedDate = c.CreatedDate,
                    UpdateDate = c.UpdateDate,
                    Description = c.Description,
                    CreatedTime = c.CreatedTime,
                    PostType = c.PostType,
                    Status = c.Status,
                    Title = c.Title,
                    Reactions = c.Reactions
                    .Select(r => new TopPostReaction()
                    {
                        Count = r.Count,
                        ReactionName = r.ReactionType.Name
                    }).ToList()
                }
                ).OrderByDescending(c =>
                c.Reactions.Where(x => x.ReactionName == "reactionLike")
                .Select(x => x.Count)
                .OrderByDescending(x => x)
                .FirstOrDefault())
                .Take(10).ToListAsync();
            var topView = await context.PostCrawls.Where(x => x.ChannelId == id)
                .Select(c => new
                {
                    Pid = c.Pid,
                    Body = c.Body,
                    CreatedDate = c.CreatedDate,
                    UpdateDate = c.UpdateDate,
                    Description = c.Description,
                    CreatedTime = c.CreatedTime,
                    PostType = c.PostType,
                    Status = c.Status,
                    Title = c.Title,

                    Reactions = c.Reactions
                    .Select(r => new TopPostReaction()
                    {
                        Count = r.Count,
                        ReactionName = r.ReactionType.Name
                    }).ToList()
                }
                ).OrderByDescending(c =>
                c.Reactions.Where(x => x.ReactionName == "reactionView")
                .Select(x => x.Count)
                .OrderByDescending(x => x)
                .FirstOrDefault())
                .Take(10).ToListAsync();
            var topComment = await context.PostCrawls.Where(x => x.ChannelId == id)
                .Select(c => new
                {
                    Pid = c.Pid,
                    Body = c.Body,
                    CreatedDate = c.CreatedDate,
                    UpdateDate = c.UpdateDate,
                    Description = c.Description,
                    CreatedTime = c.CreatedTime,
                    PostType = c.PostType,
                    Status = c.Status,
                    Title = c.Title,
                    Reactions = c.Reactions

                    .Select(r => new TopPostReaction()
                    {
                        Count = r.Count,
                        ReactionName = r.ReactionType.Name
                    }).ToList()
                }
                ).OrderByDescending(c =>
                c.Reactions.Where(x => x.ReactionName == "reactionComment")
                .Select(x => x.Count)
                .OrderByDescending(x => x)
                .FirstOrDefault())
                .Take(10).ToListAsync();
            return new Dictionary<string, object>() {
                {"like", topLike },
                {"view",topView },
                {"comment",topComment } };
        }

        public async Task<object> FilterTopPost(int id)
        {
            var channel = await context.ChannelCrawls.Where(c => c.Id == id)
               .Select(c => new ChannelStatistic()
               {
                   Id = c.Id,
                   PlatformId = c.PlatformId,
               })
               .FirstOrDefaultAsync();
            if (channel == null)
            {
                return null;
            }
            Dictionary<string, object> data = null;
            switch (channel.PlatformId)
            {
                case 1:
                    data = await FilterTopPostYoutubeChannel(id);
                    break;
                case 2:
                    data = await FilterTopPostFacebookChannel(id);
                    break;
                case 3:
                    data = await FilterTopPostTiktokChannel(id);
                    break;
                default:
                    return null;

            }
            return new
            {
                data
            };
        }

        public async Task<object> StatisticChannel()
        {
            var now = DateTime.Now.AddDays(1);
            var currentMonth = DateTime.Now.AddDays(-(DateTime.Now.Day - 1));
            var lastMonth = currentMonth.AddMonths(-1);
            var facebookCount = await context.ChannelCrawls.Where(c => c.PlatformId == 2).Select(c => c.PlatformId).CountAsync();
            var youtubeCount = await context.ChannelCrawls.Where(c => c.PlatformId == 1).Select(c => c.PlatformId).CountAsync();
            var tiktokCount = await context.ChannelCrawls.Where(c => c.PlatformId == 3).Select(c => c.PlatformId).CountAsync();
            var facebookCurrentMonth = await context.ChannelCrawls.Where(c => c.PlatformId == 2
                && c.CreatedDate >= currentMonth.Date && c.CreatedDate < now.Date)
                .Select(c => c.PlatformId).CountAsync();
            var facebookLastMonth = await context.ChannelCrawls.Where(c => c.PlatformId == 2
                && c.CreatedDate >= lastMonth.Date && c.CreatedDate < currentMonth.Date)
                .Select(c => c.PlatformId).CountAsync();
            var facebookStatus = facebookCurrentMonth > facebookLastMonth;
            var facebookPercent = (facebookLastMonth == 0) ? 100 : Math.Round((float)Math.Abs(facebookLastMonth - facebookCurrentMonth) / facebookLastMonth * 100, 2);
            var youtubeCurrentMonth = await context.ChannelCrawls.Where(c => c.PlatformId == 1
                && c.CreatedDate >= currentMonth.Date && c.CreatedDate < now.Date)
                .Select(c => c.PlatformId).CountAsync();
            var youtubeLastMonth = await context.ChannelCrawls.Where(c => c.PlatformId == 1
                && c.CreatedDate >= lastMonth.Date && c.CreatedDate < currentMonth.Date)
                .Select(c => c.PlatformId).CountAsync();
            var youtubeStatus = youtubeCurrentMonth > youtubeLastMonth;
            var youtubePercent = (youtubeLastMonth == 0) ? 100 : Math.Round((float)Math.Abs(youtubeLastMonth - youtubeCurrentMonth) / youtubeLastMonth * 100, 2);
            var tiktokCurrentMonth = await context.ChannelCrawls.Where(c => c.PlatformId == 3
                && c.CreatedDate >= currentMonth.Date && c.CreatedDate < now.Date)
                .Select(c => c.PlatformId).CountAsync();
            var tiktokLastMonth = await context.ChannelCrawls.Where(c => c.PlatformId == 3
                && c.CreatedDate >= lastMonth.Date && c.CreatedDate < currentMonth.Date)
                .Select(c => c.PlatformId).CountAsync();
            var tiktokStatus = tiktokCurrentMonth > tiktokLastMonth;
            var tiktokPercent = (tiktokLastMonth == 0) ? 100 : Math.Round((float)Math.Abs(tiktokLastMonth - tiktokCurrentMonth) / tiktokLastMonth * 100, 2);


            return new
            {
                Facebook = new
                {
                    Count = facebookCount,
                    ThisMonth = facebookCurrentMonth,
                    LastMonth = facebookLastMonth,
                    IsIncreate = facebookStatus,
                    Percent = facebookPercent
                },
                Youtube = new
                {
                    Count = youtubeCount,
                    ThisMonth = youtubeCurrentMonth,
                    LastMonth = youtubeLastMonth,
                    IsIncreate = youtubeStatus,
                    Percent = youtubePercent
                },
                Tiktok = new
                {
                    Count = tiktokCount,
                    ThisMonth = tiktokCurrentMonth,
                    LastMonth = tiktokLastMonth,
                    IsIncreate = tiktokStatus,
                    Percent = tiktokPercent
                },
            };
        }

        public async Task<object> StatisticDashboard()
        {
            var now = DateTime.Now;
            var lastWeek = DateTime.Now.AddMonths(-3);
            var result = await context.ChannelCrawls.Where(c => c.CreatedDate >= lastWeek.Date && c.CreatedDate < now.Date.AddDays(1)).GroupBy(c => c.CreatedDate.Value.Date)
                .Select(c => new
                {
                    Key = c.Key.Date,
                    Count = c.Count()
                }).ToListAsync();
            return new
            {
                Total = result.Sum(x => x.Count),
                History = result
            };
        }

        public async Task<PaginationList<ChannelCrawl>> GetChannelSchedule(List<string> concurrentJobs, HangfireChannelFilter filter)
        {
            var totalItem = await context.ChannelCrawls.Where(x => !concurrentJobs.Contains(x.Username)).ApplyFilterWithoutPagination(filter).Select(x=>x.Id).CountAsync();
            var currentPage = filter.Page;
            var pageSize = filter.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.ChannelCrawls.Where(x => !concurrentJobs.Contains(x.Username)).ApplyFilter(filter)
                .Select(x=> new ChannelCrawl()
                {
                    Id = x.Id,
                    Username = x.Username,
                    Cid = x.Cid,
                    CreatedDate = x.CreatedDate,
                    UpdateDate = (x.UpdateDate.HasValue)?x.UpdateDate: x.CreatedDate,
                    Url = x.Url,
                    PlatformId = x.LocationId,
                    Platform = new Platform() { Name = x.Platform.Name},
                    Name = x.Name,
                    ChannelRecords = null,
                    ChannelCategories = null,
                }).ToList();
            return new PaginationList<ChannelCrawl>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = (int)totalPage,
                TotalItem = totalItem,
                Items = result
            };
        }
    }
}
