using Business.Repository.GenericRepo;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.FeaturePlanRepo
{
    public class FeaturePlanRepository : GenericRepository<FeaturePlan>, IFeaturePlanRepository
    {
        public FeaturePlanRepository(SocialMediaContext context) : base(context)
        {
        }


    }
}
