using Business.Repository.BrandRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.BranModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.BrandService
{
    public class BrandService : BaseService, IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly string ClassName = typeof(Brand).Name;
        public BrandService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IBrandRepository brandRepository) : base(httpContextAccessor, userRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<bool> Delete(int id)
        {
            var brand = await GetById(id);
            if (brand != null)
            {
                var result = await _brandRepository.Delete(id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<BrandDto> GetById(int id)
        {
            var includes = new List<string>()
                {
                    nameof(Brand.Organization),
                };
            var brand = await _brandRepository.Get(x => x.Id == id, includes);
            return MapperConfig.GetMapper().Map<BrandDto>(brand);
        }

        public async Task<int> Insert(InsertBrandDto dto)
        {
            var check = await SearchByName(dto.Name);
            if (check == null)
            {
                var brand = MapperConfig.GetMapper().Map<Brand>(dto);
                var result = await _brandRepository.Insert(brand);
                return brand.Id;
            }
            else
            {
                throw new Exception(DUPLICATED+ " "+ ClassName);
            }
        }

        public Task<bool> PagingSearch()
        {
            throw new NotImplementedException();
        }

        public async Task<BrandDto> SearchByName(string name)
        {
            var brand =  await _brandRepository.Get(x => x.Name.Equals(name));
            return MapperConfig.GetMapper().Map<BrandDto>(brand);
        }

        public async Task<int> Update(UpdateBrandDto dto)
        {
            if ((await GetById(dto.Id)) == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var check = await SearchByName(dto.Name);
            if (check == null || dto.Id == check.Id)
            {
                var brand = MapperConfig.GetMapper().Map<Brand>(dto);
                var result = await _brandRepository.Update(brand);
                return brand.Id;
            }
            else
            {
                throw new Exception("Duplicated Organization name");
            }
        }
    }
}
