using API.Utils;
using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.ChannelCrawlModel.FacebookStatistic;
using DataAccess.Models.Pagination;
using DataAccess.Models.ReactionModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var resultInfo = new Z.BulkOperations.ResultInfo();

            await context.BulkMergeAsync(list, options =>
            {
                options.IncludeGraph = true;
                options.InsertIfNotExists = true;
                options.UseRowsAffected = true;
                options.ResultInfo = resultInfo;
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
            Console.WriteLine(resultInfo.RowsAffected);
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

            DateTime dateFrom = filter.CreatedTime.Min.Value;
            DateTime dateTo = filter.CreatedTime.Max.Value.AddDays(1);
            List<ChannelStatistic> channels = new List<ChannelStatistic>();
            
            foreach (string userId in userIds)
            {
                var channel = await context.ChannelCrawls
                .Where(c => (filter.Platform > 0 ? c.PlatformId == filter.Platform : true) && c.Username== userId)
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
                            Body = p.Body,
                            Description = p.Description,
                            CreatedTime = p.CreatedTime,
                            PostType = p.PostType,
                            Status = p.Status,
                            Title = p.Title,
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
            var test = await context.ChannelCrawls.FirstOrDefaultAsync(x => x.Cid == entity.Cid && (entity.Id != 0 ? x.Id != entity.Id : true));
            if ((await context.ChannelCrawls.FirstOrDefaultAsync(x => x.Cid == entity.Cid && (entity.Id != 0 ? x.Id != entity.Id : true))) != null)
            {
                throw new Exception("Duplicated channel");
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
                Items = result
            };
        }

        public async Task<Dictionary<string,object>> FilterTopPostFacebookChannel(int id)
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
            var channel =  await context.ChannelCrawls.Where(c => c.Id == id)
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
                    data =await FilterTopPostFacebookChannel(id);
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
    }
}
