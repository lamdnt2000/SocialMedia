using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ChannelRecordModel
{
    public class UpdateChannelRecordDto:InsertChannelRecordDto
    {
        public bool Status { get; set; }
    }
}
