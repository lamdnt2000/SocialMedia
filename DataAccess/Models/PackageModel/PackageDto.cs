using DataAccess.Models.FeatureModel;
using DataAccess.Models.PlanModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.PackageModel
{
    public class PackageDto :InsertPakageDto
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public ICollection<FeatureDto> Features { get; set; }
        public ICollection<PlanDto> Plans { get; set; }
    }
}
