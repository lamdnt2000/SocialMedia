using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.ChannelRecordRepo
{
    public class ChannelRecordRepository : GenericRepository<ChannelRecord>, IChannelRecordRepository
    {
        public ChannelRecordRepository(SocialMediaContext context) : base(context)
        {
        }

      
    }
}
