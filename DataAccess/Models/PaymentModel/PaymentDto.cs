using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.PaymentModel
{
    public class PaymentDto
    {
        [Required]
        [Range(50000,10000000)]
        
        public int Money { get; set; }
    }
}
