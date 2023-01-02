using AutoFilterer.Attributes;
using AutoFilterer.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.TransectionDepositModel
{
    public class TransactionDepositPaging : PaginationFilterBase
    {

        public int WalletId { get; set; }
        
        public bool Status { get; set; }
    }
}
