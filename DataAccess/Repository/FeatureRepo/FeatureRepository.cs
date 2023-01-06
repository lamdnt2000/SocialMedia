using Business.Repository.GenericRepo;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.BulkOperations;

namespace DataAccess.Repository.FeatureRepo
{
    public class FeatureRepository : GenericRepository<Feature>, IFeatureRepository
    {
        public FeatureRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<ICollection<int>> BulkInsertOrUpdate(ICollection<Feature> features)
        {
            await context.BulkMergeAsync(features, options =>
            {
                options.IncludeGraph = true;
                options.InsertIfNotExists = true;
                options.IncludeGraphOperationBuilder = operation =>
                {
                    if (operation is BulkOperation<Feature> bulkFeature)
                    {
                        bulkFeature.ColumnPrimaryKeyExpression = expression => new { expression.Id };
                    }
                };
            });

            return features.Select(x => x.Id).ToList();
        }
    }
}
