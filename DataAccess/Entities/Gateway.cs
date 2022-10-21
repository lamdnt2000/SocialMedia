using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Gateway
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
        public string BankCode { get; set; }
        public string BankTransNo { get; set; }
        public int AdminId { get; set; }

        public virtual Admin Admin { get; set; }
    }
}
