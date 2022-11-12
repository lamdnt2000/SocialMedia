using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.WalletModel
{
    public class InsertWalletDto
    {
        [Required]
        public string Currency { get; set; }
        public string Balance { get; set; }
        public string Status { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
