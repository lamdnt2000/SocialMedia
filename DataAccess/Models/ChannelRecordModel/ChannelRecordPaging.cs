using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelRecordModel
{
    public class ChannelRecordPaging: PaginationFilterBase
    {
        public int ChannelId { get; set; }
        public Range<DateTime> Offset { get; set; }
        public bool Status { get; set; }
    }
}
