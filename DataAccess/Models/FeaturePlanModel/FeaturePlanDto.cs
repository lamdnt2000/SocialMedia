using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DataAccess.Models.FeaturePlanModel
{
    public class FeaturePlanDto
    {
        public int PlanId { get; set; }
        [Required]
        public int FeatureId { get; set; }
        [ReadOnly(true)]
        public int FeatureType { get; set; }
        [ReadOnly(true)]
        public string FeatureName { get; set; }
        public int? Quota { get; set; } = 0;
        public bool Valid { get; set; } = true;
    }
}
