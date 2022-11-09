using DataAccess.Models.LocationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.ChannelCrawlService
{
    public interface IChannelCrawlService
    {
        Task<int> Insert(InsertLocationDto dto);
        Task<int> Update(int id, UpdateLocationDto dto);
        Task<bool> Delete(int id);
        Task<LocationDto> GetById(int id);
    }
}
