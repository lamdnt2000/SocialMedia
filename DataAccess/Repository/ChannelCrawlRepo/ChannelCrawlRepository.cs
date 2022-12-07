using API.Utils;
using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.Pagination;
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
            var channel = await context.ChannelCrawls.Where(c => c.Id == filter.Id)
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
                            CreatedDate = p.CreatedDate,
                            UpdateDate = p.UpdateDate,
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
            var result = context.ChannelCrawls.ApplyFilter(paging).Include(c => c.Organization).ToList();
            return new PaginationList<ChannelCrawl>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = (int)totalPage,
                Items = result
            };
        }
    }
}
