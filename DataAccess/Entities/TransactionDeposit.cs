using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class TransactionDeposit
    {
        public int Id { get; set; }
        public string CardType { get; set; }
        public int Amount { get; set; }
        public int Status { get; set; }
        public string Code { get; set; }
        public string TransNoId { get; set; }
        public DateTime PayDate { get; set; }
        public string Locale { get; set; }
        public string OrderInfor { get; set; }
        public string OrderType { get; set; }
        public string TxnRef { get; set; }
        public int WalletId { get; set; }
        public int GatewayId { get; set; }
        public string TmnCode { get; set; }
        public int CurrentBlance { get; set; }
        public int NewBalance { get; set; }

        public virtual Wallet Wallet { get; set; }
    }
}
