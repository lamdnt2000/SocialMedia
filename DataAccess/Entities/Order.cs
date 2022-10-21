using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Order
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public int TotalMoney { get; set; }
        public string ChannelId { get; set; }
        public int MemberId { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Member Member { get; set; }
    }
}
