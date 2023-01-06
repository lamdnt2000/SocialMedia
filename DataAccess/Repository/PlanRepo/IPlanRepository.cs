using Business.Repository.GenericRepo;
using DataAccess.Entities;
using DataAccess.Models.PlanModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.PlanRepo
{
    public interface IPlanRepository:IGenericRepository<Plan>
    {
        Task<ICollection<int>> BulkInsertOrUpdate(ICollection<Plan> plans);
        Task<PlanDto> GetPlanById(int planId);
        Task<string> ConvertPlanFeatureToJson(int plainId);

    }
}
