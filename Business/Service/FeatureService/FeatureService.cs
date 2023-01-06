using Business.Repository.PackageRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Enum;
using DataAccess.Models.FeatureModel;
using DataAccess.Repository.FeaturePlanRepo;
using DataAccess.Repository.FeatureRepo;
using DataAccess.Repository.PlanRepo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;


namespace Business.Service.FeatureService
{
    public class FeatureService:BaseService, IFeatureService
    {
        private readonly IFeatureRepository _featureRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IFeaturePlanRepository _featurePlanRepository;
        private string ClassName = typeof(Feature).Name;
        private string SubClassName = typeof(Package).Name;
        public FeatureService(IHttpContextAccessor httpContextAccessor
            , IUserRepository userRepository
            , IFeatureRepository featureRepository
            , IPackageRepository packageRepository
            , IFeaturePlanRepository featurePlanRepository
            , IPlanRepository planRepository) : base(httpContextAccessor, userRepository)
        {
            _featureRepository = featureRepository;
            _packageRepository = packageRepository;
            _featurePlanRepository = featurePlanRepository;
            _planRepository = planRepository;
        }

        private async Task<FeatureDto> GetFeatureById(int featureId)
        {
            var result = await _featureRepository.Get(x => x.Id == featureId);
            return (result != null) ? MapperConfig.GetMapper().Map<FeatureDto>(result) : null;
        }

        public async Task<bool> Delete( int featureId)
        {
            var feature = await GetFeatureById(featureId);
            if (feature != null)
            {
                var featurPlans = await _featurePlanRepository.GetAllAsync(x => x.FeatureId == featureId);
                if (featurPlans.Count() > 0)
                {
                    await _featurePlanRepository.DeleteRange(featurPlans);
                }
                var result = await _featureRepository.Delete(featureId);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<ICollection<int>> RangeInsertOrUpdate(int packageId, ICollection<FeatureDto> features)
        {
            var package = await _packageRepository.Get(x => x.Id == packageId);
            if (package == null)
            {
                throw new Exception(SubClassName + " " + NOT_FOUND);
            }
            var data = MapperConfig.GetMapper().Map<ICollection<Feature>>(features);

            foreach (Feature f in data)
            {
                f.PackageId = packageId;
            }
            var result = await _featureRepository.BulkInsertOrUpdate(data);
            if (result.Count > 0)
            {
                var plans = await _planRepository.GetAllAsync(x => x.PackageId == packageId);
                List<FeaturePlan> featurePlans = new List<FeaturePlan>();
                foreach (Plan p in plans)
                {
                    foreach (int fId in result)
                    {
                        var featurePlan = new FeaturePlan() { FeatureId = fId, PlanId = p.Id, Quota=0, Valid=true};
                        featurePlans.Add(featurePlan);
                    }
                }
                if (featurePlans.Count > 0)
                {
                    await _featurePlanRepository.InsertRange(featurePlans);
                }
            }
            return result;
            
        }

        public ICollection<FeatureDto> ValidFeature()
        {
            List<FeatureDto> result = new List<FeatureDto>();
            FeatureDto youtube = new FeatureDto() { Name="YOUTUBE", Description = "YOUTUBE", Type = EnumFeatureType.FEATURE, Status = true};
            FeatureDto facebook = new FeatureDto() { Name="FACEBOOK", Description = "FACEBOOK", Type = EnumFeatureType.FEATURE, Status = true };
            FeatureDto tiktok = new FeatureDto() { Name="TIKTOK", Description = "TIKTOK", Type = EnumFeatureType.FEATURE, Status = true };
            FeatureDto dailySearch = new FeatureDto() { Name="DAILYSEARCH", Description = "DAILYSEARCH", Type = EnumFeatureType.QUANTITY, Status = true };
            FeatureDto dailyRequest = new FeatureDto() { Name="DAILYREQUEST", Description = "DAILYREQUEST", Type = EnumFeatureType.QUANTITY, Status = true };
            FeatureDto isCompare = new FeatureDto() { Name="COMPARE", Description = "COMPARE", Type = EnumFeatureType.FEATURE, Status = true };
            result.Add(youtube);
            result.Add(facebook);
            result.Add(tiktok);
            result.Add(dailySearch);
            result.Add(dailyRequest);
            result.Add(isCompare);
            return result;
        }

        public async Task<ICollection<int>> Insert(int packageId, EnumFeature feature)
        {
            var features = ValidFeature();
            var dto = features.ToArray()[(int)Enum.Parse<EnumFeature>(feature.ToString())];
            var check = await GetFeatureByName(packageId, dto.Name);
            if (check != null)
            {
                throw new Exception(ClassName + " " + DUPLICATED);
            }
            return await RangeInsertOrUpdate(packageId, new List<FeatureDto>() { dto });
        }

        private async Task<Feature> GetFeatureByName(int packageId, string name)
        {
            return await _featureRepository.Get(x => x.PackageId == packageId && x.Name == name);
        }
    }
}
