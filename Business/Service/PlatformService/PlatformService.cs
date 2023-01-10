using Business.Repository.PlatformRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.Pagination;
using DataAccess.Models.PlatFormModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.PlatformService
{
    public class PlatformService : IPlatformService
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly string ClassName = typeof(Platform).Name;

        public PlatformService(
            IPlatformRepository platformRepository)
        {
            _platformRepository = platformRepository;
        }

        public async Task<bool> Delete(int id)
        {
            var platform = await GetById(id);
            if (platform != null)
            {
                var result = await _platformRepository.Delete(id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<PlatformDto> GetById(int id, bool isInclude = false)
        {
            var includes = new List<string>();
            if (isInclude)
            {
                includes.Add(nameof(Platform.Categories));
            }
            var platform = await _platformRepository.Get(x => x.Id == id, includes);
            return MapperConfig.GetMapper().Map<PlatformDto>(platform);
        }

        public async Task<int> Insert(InsertPlatformDto dto)
        {
            var check = await SearchByName(dto.Name);
            if (check == null)
            {
                var platform = MapperConfig.GetMapper().Map<Platform>(dto);
                platform.Status = 1;
                var result = await _platformRepository.Update(platform);
                return platform.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }


        public async Task<PaginationList<PlatformDto>> SearchAsync(PlatformPaging paging)
        {
            var result = await _platformRepository.SearchPlatformAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<PlatformDto>>(result.Items);
            return new PaginationList<PlatformDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalItem = result.TotalItem,
                TotalPage = result.TotalPage
            };
        }

        public async Task<PlatformDto> SearchByName(string name)
        {
            var platform = await _platformRepository.Get(x => x.Name.Equals(name));
            return MapperConfig.GetMapper().Map<PlatformDto>(platform);
        }

        public async Task<int> Update(int id, UpdatePlatformDto dto)
        {
            var check = await SearchByName(dto.Name);
            if ((await GetById(id) == null))
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            if (check == null || id == check.Id)
            {
                var platform = MapperConfig.GetMapper().Map<Platform>(dto);
                platform.Id = id;
                var result = await _platformRepository.Update(platform);
                return platform.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }
    }
}
