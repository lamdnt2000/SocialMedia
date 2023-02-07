using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.SubscriptionModel
{
    public class PlanPaymentDto
    {
        [Required]
        public int PlanId { get; set; }
        [Required]
        [EnumDataType(typeof(EnumPlanPrice))]
        public int PriceType { get; set; }
        
    }
}
