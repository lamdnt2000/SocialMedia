using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.TransectionDepositModel
{
    public class InsertTransactionDepositDto
    {
        [Required]
        [StringLength(10)]
        public string CardType { get; set; }
        public int Amount { get; set; }
        [Required]
        public string Code { get; set; }
        public int Status { get; set; }
        [Required]
        public int WalletId { get; set; }
        [Required]
        public int GatewayId { get; set; }
    }
}
