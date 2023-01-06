using Business.Repository.PackageRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.FeatureModel;
using DataAccess.Models.FeaturePlanModel;
using DataAccess.Models.PlanModel;
using DataAccess.Models.PriceModel;
using DataAccess.Repository.FeatureRepo;
using DataAccess.Repository.PlanRepo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.PlanService
{
    public class PlanService : BaseService, IPlanService
    {
        private readonly IPlanRepository _planRepository;
        private readonly IFeatureRepository _featureRepository;
        private readonly IPackageRepository _packageRepository;
        private string SubClassName = typeof(Package).Name;
        private string FeatureName = typeof(Feature).Name;
        private string ClassName = typeof(Plan).Name;
        public PlanService(IHttpContextAccessor httpContextAccessor
            , IUserRepository userRepository
            , IPlanRepository planRepository
            , IPackageRepository packageRepository
            , IFeatureRepository featureRepository) : base(httpContextAccessor, userRepository)
        {
            _planRepository = planRepository;
            _packageRepository = packageRepository;
            _featureRepository = featureRepository;
        }
        private async Task<PlanDto> GetPlanById(int plantId)
        {
            var result = await _planRepository.Get(x => x.Id == plantId);
            return (result != null) ? MapperConfig.GetMapper().Map<PlanDto>(result) : null;
        }
        public async Task<bool> Delete(int planId)
        {
            var feature = await GetPlanById(planId);
            if (feature != null)
            {
                var result = await _planRepository.Delete(planId);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }



        public async Task<ICollection<int>> RangeInsertOrUpdate(int packageId, ICollection<InsertPlanDto> plans)
        {
            var package = await _packageRepository.Get(x => x.Id == packageId);
            if (package == null)
            {
                throw new Exception(SubClassName + " " + NOT_FOUND);
            }
            var data = MapperConfig.GetMapper().Map<ICollection<Plan>>(plans);

            foreach (Plan p in data)
            {
                p.PackageId = packageId;
                foreach (FeaturePlan f in p.FeaturePlans)
                {
                    f.Feature = null;
                }

            }
            return await _planRepository.BulkInsertOrUpdate(data);
        }

        public async Task<ICollection<int>> InsertPlanByName(int packageId, string planName)
        {
            var check = await GetPlanByName(packageId, planName);
            if (check != null)
            {
                throw new Exception(ClassName + " " + DUPLICATED);
            }
            var features = await _featureRepository.GetAllAsync(x => x.PackageId == packageId);
            if (features != null)
            {
                List<FeaturePlanDto> featurePlans = new List<FeaturePlanDto>();
                foreach (Feature f in features)
                {
                    FeaturePlanDto featurePlan = new FeaturePlanDto() { FeatureId = f.Id, Valid = true, Quota = 0 };
                    featurePlans.Add(featurePlan);
                }
                List<PriceDto> prices = new List<PriceDto>()
                {
                    new PriceDto() { Price = 0, Quantity=1, PriceType=1 },
                    new PriceDto() { Price = 0, Quantity=1, PriceType=2 },
                    new PriceDto() { Price = 0, Quantity=1, PriceType=3 },
                };
                InsertPlanDto dto = new InsertPlanDto()
                {
                    Name = planName,
                    Description = planName
                    ,
                    Status = true,
                    PackageId = packageId
                    ,
                    FeaturePlans = featurePlans,
                    PlanPrices = prices
                };
                return await RangeInsertOrUpdate(packageId, new List<InsertPlanDto>() { dto });
            }
            else
            {
                throw new Exception(FeatureName + " " + NOT_FOUND);
            }



        }

        public async Task<PlanDto> GetPlan(int planId)
        {
            return await _planRepository.GetPlanById(planId);
        }

        public async Task<Plan> GetPlanByName(int packageId, string name)
        {
            return await _planRepository.Get(x => (x.PackageId > 0 ? x.PackageId == packageId : true) && x.Name == name);
        }

        public async Task<ICollection<int>> UpdatePlan(int planId, InsertPlanDto plan)
        {
            var checkId = await GetPlan(planId);
            if (checkId == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var checkName = await GetPlanByName(checkId.PackageId, plan.Name);
            if (checkName != null)
            {
                throw new Exception(ClassName + " " + DUPLICATED);
            }
            return await RangeInsertOrUpdate(plan.PackageId, new List<InsertPlanDto>() { plan });
        }
    }
}
