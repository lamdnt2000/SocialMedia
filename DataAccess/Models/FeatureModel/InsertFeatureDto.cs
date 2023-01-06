using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Enum;

namespace DataAccess.Models.FeatureModel
{
    public class InsertFeatureDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        [EnumDataType(typeof(EnumFeatureType))]
        public EnumFeatureType Type { get; set; }
        [Required]
        public int PackageId { get; set; }
    }
}
