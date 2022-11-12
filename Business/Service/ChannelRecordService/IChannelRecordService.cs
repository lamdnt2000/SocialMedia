using DataAccess.Entities;
using DataAccess.Models.CategoryModel;
using DataAccess.Models.ChannelRecordModel;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.ChannelRecordService
{
    public interface IChannelRecordService
    {
        Task<int> Insert(InsertChannelRecordDto dto);
        Task<int> Update(int id, UpdateChannelRecordDto dto);
        Task<bool> Delete(int id);
        Task<ChannelRecordDto> GetById(int id);
        Task<PaginationList<ChannelRecordDto>> SearchAsync(ChannelRecordPaging paging);
    }
}
