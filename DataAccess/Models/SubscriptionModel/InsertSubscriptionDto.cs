using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.SubscriptionModel
{
    public class InsertSubscriptionDto
    {
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        [Required]
        public int WalletId { get; set; }
        [Required]
        public int PackageId { get; set; }
        [Required]
        public int OfferId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
