using Business.Repository.GenericRepo;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.ChannelRecordRepo
{
    public interface IChannelRecordRepository: IGenericRepository<ChannelRecord>
    {
        bool ValidEntity(ChannelRecord entity);
    }
}
