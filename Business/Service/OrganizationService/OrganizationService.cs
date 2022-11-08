using Business.Repository.OrganizationRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.CategoryModel;
using DataAccess.Models.OrganizationModel;
using DataAccess.Models.Pagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.OrganizationService
{
    public class OrganizationService : BaseService, IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly string ClassName = typeof(Organization).Name;

        public OrganizationService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IOrganizationRepository organizationRepository) : base(httpContextAccessor, userRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<bool> Delete(int id)
        {
            var organization = await GetById(id);
            if (organization != null)
            {
                var result = await _organizationRepository.Delete(id);
                return (result > 0) ? true : false;
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<OrganizationDto> GetById(int id, bool isInclude = false)
        {
            var includes = new List<string>();
            if (isInclude)
            {
                includes.Add(nameof(Organization.Brands));
            }
            var organization =  await _organizationRepository.Get(x => x.Id == id, includes);
            return MapperConfig.GetMapper().Map<OrganizationDto>(organization);
        }

        public async Task<int> Insert(InsertOrganizationDto dto)
        {
            var check = await SearchByName(dto.Name);
            if (check == null)
            {
                var organization = MapperConfig.GetMapper().Map<Organization>(dto);
                organization.CreatedDate = DateTime.Now;
                organization.Status = true;
                var result = await _organizationRepository.Insert(organization);
                return organization.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }

        public async Task<PaginationList<OrganizationDto>> SearchAsync(OrganizationPaging paging)
        {
            var result = await _organizationRepository.SearchAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<OrganizationDto>>(result.Items);
            return new PaginationList<OrganizationDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalPage = result.TotalPage
            };
        }

        public async Task<OrganizationDto> SearchByName(string name)
        {
            var organization =  await _organizationRepository.Get(x => x.Name.Equals(name));
            return MapperConfig.GetMapper().Map<OrganizationDto>(organization);
        }

        public async Task<int> Update(int id, UpdateOrganizationDto dto)
        {
            var check = await SearchByName(dto.Name);
            if ((await GetById(id)) == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            if (check == null || id == check.Id)
            {
                var organization = MapperConfig.GetMapper().Map<Organization>(dto);
                organization.Id = id;
                organization.UpdateDate = DateTime.Now;
                var result = await _organizationRepository.Update(organization);
                return organization.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }


    }
}
