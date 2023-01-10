using Business.Repository.PackageRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.PackageModel;
using DataAccess.Models.Pagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.PakageService
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly string ClassName = typeof(Package).Name;

        public PackageService(
            IPackageRepository packageRepository)
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

        public async Task<ICollection<PackageDto>> GetAll()
        {
            return await _packageRepository.GetAll();
        }

        public async Task<PackageDto> GetById(int id)
        {
            var package = await _packageRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<PackageDto>(package);
        }

        public async Task<PackageDto> GetPackageInclude(int id)
        {
            var package = await _packageRepository.Get(x => x.Id == id, new List<string>() { "Features" });
            return MapperConfig.GetMapper().Map<PackageDto>(package);
        }

        public async Task<PackageDto> GetPlansOfPackage(int id)
        {
            return await _packageRepository.GetPlanOfPackage(id);

        }

        public async Task<int> Insert(InsertPakageDto dto)
        {
            var check = await SearchByName(dto.Name);
            if (check == null)
            {
                var package = MapperConfig.GetMapper().Map<Package>(dto);
                package.Status = true;
                var result = await _packageRepository.Insert(package);
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
                TotalItem = result.TotalItem,
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
