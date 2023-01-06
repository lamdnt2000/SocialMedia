using Business.Repository.GenericRepo;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.FeatureRepo
{
    public interface IFeatureRepository:IGenericRepository<Feature>
    {
        Task<ICollection<int>> BulkInsertOrUpdate(ICollection<Feature> features);
    }
}
