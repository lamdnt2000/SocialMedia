using DataAccess.Models.FeatureModel;
using DataAccess.Models.PlanModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.PlanService
{
    public interface IPlanService
    {
        Task<ICollection<int>> RangeInsertOrUpdate(int packageId, ICollection<InsertPlanDto> plans);
        Task<ICollection<int>> InsertPlanByName(int packageId, string planName);
        Task<ICollection<int>> UpdatePlan(int planId, InsertPlanDto plan);
        Task<PlanDto> GetPlan(int planId);
        Task<bool> Delete(int featureId);
        
    }
}
