using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.TransectionDepositModel
{
    public class TransactionDepositDto
    {
        public int Id { get; set; }
        public long TxnRef { get; set; }
        public DateTime PayDate { get; set; }
        public int Amount { get; set; }
        public bool Status { get; set; }
        
    }
}
