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

namespace Business.Repository.ChannelCrawlRepo
{
    public class ChannelCrawlRepository : GenericRepository<ChannelCrawl>, IChannelCrawlRepository
    {
        public ChannelCrawlRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<ChannelCrawl> FilterChannel(ChannelFilter filter)
        {
            DateTime dateFrom = filter.CreatedTime.Min.Value;
            DateTime dateTo = filter.CreatedTime.Max.Value.AddDays(1);
            var channel =await context.ChannelCrawls.Where(c => c.Id == filter.Id )
                .Include(c => c.ChannelRecords)
                .Include(c => c.PostCrawls.Where(p => p.CreatedTime >= dateFrom && p.CreatedTime < dateTo)
                .OrderBy(p=>p.CreatedTime))
                .ThenInclude(p => p.Reactions).ThenInclude(r => r.ReactionType)
                .FirstOrDefaultAsync();
            return channel;
           

        }

        public bool ValidateChannel(ChannelCrawl entity)
        {
            if (!context.Organizations.Any(x => x.Id == entity.OrganizationId))
            {
                throw new Exception("Organization not exist!");
            }
            if (entity.BrandId!=0)
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
            if (context.ChannelCrawls.Any(x => x.Id != entity.Id && x.Cid == entity.Cid))
            {
                throw new Exception("Duplicated channel");
            }

            return true;
        }
    }
}
