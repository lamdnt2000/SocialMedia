using DataAccess.Enum;
using DataAccess.Models.FeatureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.FeatureService
{
    public interface IFeatureService
    {
        Task<ICollection<int>> RangeInsertOrUpdate(int packageId, ICollection<FeatureDto> features);
        Task<ICollection<int>> Insert(int pacageId, EnumFeature feature);
        Task<bool> Delete(int featureId);
        ICollection<FeatureDto> ValidFeature();
    }
}
