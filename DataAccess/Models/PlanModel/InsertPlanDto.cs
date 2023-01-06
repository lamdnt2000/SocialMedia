using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models.FeaturePlanModel;
using DataAccess.Models.PriceModel;

namespace DataAccess.Models.PlanModel
{
    public class InsertPlanDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int PackageId { get; set; }
        public bool Status { get; set; } = true;
        public ICollection<FeaturePlanDto> FeaturePlans { get; set; }
        public ICollection<PriceDto> PlanPrices { get; set; }

    }
}
