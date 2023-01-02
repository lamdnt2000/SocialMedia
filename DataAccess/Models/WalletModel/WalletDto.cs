using DataAccess.Models.TransectionDepositModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.WalletModel
{
    public class WalletDto
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public bool Status { get; set; }
        public string Currency { get; set; }
        public int UserId { get; set; }
    }
}
