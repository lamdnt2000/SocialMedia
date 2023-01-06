using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Enum;

namespace DataAccess.Models.PriceModel
{
    public class PriceDto
    {
        
        public int Id { get; set; }
        [Required]
        [EnumDataType(typeof(EnumPlanPrice))]
        public int PriceType { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int PlanId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
