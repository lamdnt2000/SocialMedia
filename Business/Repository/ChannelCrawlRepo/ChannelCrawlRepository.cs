using Business.Repository.GenericRepo;
using Business.Utils;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.ChannelCrawlModel;
using Google.Api.Gax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.BulkOperations;
using Z.EntityFramework.Extensions;

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

        public async Task<ChannelCrawl> FilterChannel(ChannelFilter filter)
        {
            DateTime dateFrom = filter.CreatedTime.Min.Value;
            DateTime dateTo = filter.CreatedTime.Max.Value.AddDays(1);
            var channel = await context.ChannelCrawls.Where(c => c.Id == filter.Id)
                .Include(c => c.ChannelRecords)
                .Include(c => c.PostCrawls.Where(p => p.CreatedTime >= dateFrom && p.CreatedTime < dateTo)
                .OrderBy(p => p.CreatedTime))
                .ThenInclude(p => p.Reactions).ThenInclude(r => r.ReactionType)
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
    }
}
