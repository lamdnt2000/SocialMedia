using Business.Repository.BrandRepo;
using Business.Repository.OrganizationRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.OrganizationModel;
using DataAccess.Models.Pagination;
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
        private readonly IOrganizationRepository _organizationRepository;

        private readonly string ClassName = typeof(Brand).Name;
        private readonly string ReferClassName = typeof(Organization).Name;

        public BrandService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IBrandRepository brandRepository,
            IOrganizationRepository organizationRepository) : base(httpContextAccessor, userRepository)
        {
            _brandRepository = brandRepository;
            _organizationRepository = organizationRepository;
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

            var brand = await _brandRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<BrandDto>(brand);
        }

        public async Task<int> Insert(InsertBrandDto dto)
        {
            if (!(await ValidOrganization(dto.OrganizationId)))
            {
                throw new Exception(ReferClassName + " " + NOT_FOUND);
            }
            var check = await SearchByName(dto.Name);
            if (check == null)
            {
                var brand = MapperConfig.GetMapper().Map<Brand>(dto);
                brand.CreatedDate = DateTime.Now;
                brand.Status = true;
                var result = await _brandRepository.Update(brand);
                return brand.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }

        public async Task<PaginationList<BrandDto>> SearchAsync(BrandPaging paging)
        {
            var result = await _brandRepository.SearchAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<BrandDto>>(result.Items);
            return new PaginationList<BrandDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalPage = result.TotalPage
            };
        }

        public async Task<BrandDto> SearchByName(string name)
        {
            var brand = await _brandRepository.Get(x => x.Name.Equals(name));
            return MapperConfig.GetMapper().Map<BrandDto>(brand);
        }

        public async Task<int> Update(int id, UpdateBrandDto dto)
        {
            if (!(await ValidOrganization(dto.OrganizationId)))
            {
                throw new Exception(ReferClassName + " " + NOT_FOUND);
            }
            if ((await GetById(id)) == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var check = await SearchByName(dto.Name);
            if (check == null || id == check.Id)
            {
                var brand = MapperConfig.GetMapper().Map<Brand>(dto);
                brand.UpdateDate = DateTime.Now;
                brand.Id = id;
                var result = await _brandRepository.Update(brand);
                return brand.Id;
            }
            else
            {
                throw new Exception("Duplicated Organization name");
            }
        }

        public async Task<bool> ValidOrganization(int id)
        {
            var organization = await _organizationRepository.Get(x => x.Id == id);
            return organization != null;
        }
    }
}
