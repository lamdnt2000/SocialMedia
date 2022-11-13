using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using Google.Api.Gax;
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

        public bool ValidateChannel(ChannelCrawl entity)
        {
            if (!context.Organizations.Any(x => x.Id == entity.OrganizationId))
            {
                throw new Exception("Organization not exist!");
            }
            if (entity.BrandId.HasValue)
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
