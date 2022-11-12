using Business.Repository.GatewayRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.GatewayModel;
using DataAccess.Models.Pagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.GatewayService
{
    public class GatewayService : BaseService, IGatewayService
    {
        private readonly IGatewayRepository _gatewayRepository;
        private readonly string ClassName = typeof(Gateway).Name;

        public GatewayService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IGatewayRepository gatewayRepository) : base(httpContextAccessor, userRepository)
        {
            _gatewayRepository = gatewayRepository;
        }
        public async Task<bool> Delete(int id)
        {
            var platform = await GetById(id);
            if (platform != null)
            {
                var result = await _gatewayRepository.Delete(id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<GatewayDto> GetById(int id, bool isInclude = false)
        {
            var includes = new List<string>();
            if (isInclude)
            {
                includes.Add(nameof(Gateway.TransactionDeposits));
            }
            var gateway = await _gatewayRepository.Get(x => x.Id == id, includes);
            return MapperConfig.GetMapper().Map<GatewayDto>(gateway);
        }

        public async Task<int> Insert(InsertGatewayDto dto)
        {
            var check = await SearchByName(dto.Name);
            if (check == null)
            {
                var gateway = MapperConfig.GetMapper().Map<Gateway>(dto);
                gateway.Status = 1;
                var result = await _gatewayRepository.Insert(gateway);
                return gateway.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }

        public async Task<PaginationList<GatewayDto>> SearchAsync(GatewayPaging paging)
        {
            var result = await _gatewayRepository.SearchGatewayAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<GatewayDto>>(result.Items);
            return new PaginationList<GatewayDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalPage = result.TotalPage
            };
        }

        public async Task<GatewayDto> SearchByName(string name)
        {
            var gateway = await _gatewayRepository.Get(x => x.Name.Equals(name));
            return MapperConfig.GetMapper().Map<GatewayDto>(gateway);
        }

        public async Task<int> Update(int id, UpdateGatewayDto dto)
        {
            var check = await SearchByName(dto.Name);
            if ((await GetById(id) == null))
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            if (check == null || id == check.Id)
            {
                var gateway = MapperConfig.GetMapper().Map<Gateway>(dto);
                gateway.Id = id;
                var result = await _gatewayRepository.Update(gateway);
                return gateway.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }
    }
}
