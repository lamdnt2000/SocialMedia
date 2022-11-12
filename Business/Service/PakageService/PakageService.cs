﻿using Business.Repository.PackageRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.PackageModel;
using DataAccess.Models.Pagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.PakageService
{
    public class PakageService : BaseService, IPakageService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly string ClassName = typeof(Package).Name;

        public PakageService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IPackageRepository packageRepository) : base(httpContextAccessor, userRepository)
        {
            _packageRepository = packageRepository;
        }

        public async Task<bool> Delete(int id)
        {
            var platform = await GetById(id);
            if (platform != null)
            {
                var result = await _packageRepository.Delete(id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<PackageDto> GetById(int id)
        {
            var package = await _packageRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<PackageDto>(package);
        }

        public async Task<int> Insert(InsertPakageDto dto)
        {
            var check = await SearchByName(dto.Name);
            if (check == null)
            {
                var package = MapperConfig.GetMapper().Map<Package>(dto);
                var result = await _packageRepository.Update(package);
                return package.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }

        public async Task<PaginationList<PackageDto>> SearchAsync(PakagePaging paging)
        {
            var result = await _packageRepository.SearchPackageAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<PackageDto>>(result.Items);
            return new PaginationList<PackageDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalPage = result.TotalPage
            };
        }

        public async Task<PackageDto> SearchByName(string name)
        {
            var package = await _packageRepository.Get(x => x.Name.Equals(name));
            return MapperConfig.GetMapper().Map<PackageDto>(package);
        }

        public async Task<int> Update(int id, UpdatePakageDto dto)
        {
            var check = await SearchByName(dto.Name);
            if ((await GetById(id) == null))
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            if (check == null || id == check.Id)
            {
                var package = MapperConfig.GetMapper().Map<Package>(dto);
                package.Id = id;
                var result = await _packageRepository.Update(package);
                return package.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }
    }
}
