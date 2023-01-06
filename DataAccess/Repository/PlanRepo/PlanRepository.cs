using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.FeaturePlanModel;
using DataAccess.Models.PackageModel;
using DataAccess.Models.Pagination;
using DataAccess.Models.PlanModel;
using DataAccess.Models.PriceModel;
using DataAccess.Models.TransectionDepositModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.BulkOperations;

namespace DataAccess.Repository.PlanRepo
{
    public class PlanRepository : GenericRepository<Plan>, IPlanRepository
    {
        public PlanRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<ICollection<int>> BulkInsertOrUpdate(ICollection<Plan> plans)
        {
            await context.BulkMergeAsync(plans, options =>
            {
                options.IncludeGraph = true;
                options.InsertIfNotExists = true;
                options.IncludeGraphOperationBuilder = operation =>
                {
                    if (operation is BulkOperation<Plan> bulkPlan)
                    {
                        bulkPlan.ColumnPrimaryKeyExpression = expression => new { expression.Id };
                        bulkPlan.IgnoreOnMergeUpdateExpression = e => new { e.CreatedDate };
                        bulkPlan.IgnoreOnMergeInsertExpression = c => new { c.UpdateDate };
                    }
                    if (operation is BulkOperation<FeaturePlan> bulkFeaturePlan)
                    {
                        bulkFeaturePlan.ColumnPrimaryKeyExpression = expression => new { expression.PlanId, expression.FeatureId };

                        
                    }if (operation is BulkOperation<PlanPrice> bulkPrice)
                    {
                        bulkPrice.ColumnPrimaryKeyExpression = expression => new { expression.Id };
                        
                    }
                };
            });
            return plans.Select(x => x.Id).ToList();
        }

        public async Task<string> ConvertPlanFeatureToJson(int plainId)
        {
            var plan = await context.Plans.Where(x => x.Id == plainId)
                .Select(x => x.FeaturePlans.Select(x => new
                {
                    Name = x.Feature.Name,
                    Type = x.Feature.Type,
                    Quota = x.Quota,
                    Valid = x.Valid
                }).ToList()
                    ).FirstOrDefaultAsync();
            Dictionary<string, object> map = new Dictionary<string, object>();
            plan.ForEach(x => map.Add(x.Name, x));
            return Newtonsoft.Json.JsonConvert.SerializeObject(map);
        }

        public async Task<PlanDto> GetPlanById(int planId)
        {
            return await context.Plans.Where(x => x.Id == planId)

                .Select(x => new PlanDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    PackageId = x.PackageId,
                    Status = x.Status,
                    CreatedDate = x.CreatedDate.Value,
                    UpdateDate = x.UpdateDate.Value,
                    PlanPrices = x.PlanPrices.Select(x => new PriceDto()
                    {
                        Id = x.Id,
                        PriceType = x.PriceType,
                        Price = x.Price,
                        Quantity = x.Quantity,
                        PlanId = x.PlanId,
                    }).ToList(),
                    FeaturePlans = x.FeaturePlans.Select(x => new FeaturePlanDto()
                    {
                        FeatureId = x.FeatureId,
                        PlanId = x.PlanId,
                        Quota = x.Quota,
                        Valid = x.Valid,
                        FeatureType = x.Feature.Type,
                        FeatureName = x.Feature.Name
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        
    }
}
