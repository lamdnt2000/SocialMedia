﻿using Business.Repository.OrganizationRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.OrganizationModel;
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

        public async Task<OrganizationDto> GetById(int id)
        {
            var includes = new List<string>()
                {
                    nameof(Organization.Brands),
                };
            var organization =  await _organizationRepository.Get(x => x.Id == id, includes);
            return MapperConfig.GetMapper().Map<OrganizationDto>(organization);
        }

        public async Task<int> Insert(InsertOrganizationDto dto)
        {
            var check = await SearchByName(dto.Name);
            if (check == null)
            {
                var organization = MapperConfig.GetMapper().Map<Organization>(dto);
                var result = await _organizationRepository.Insert(organization);
                return organization.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }

        public Task<bool> PagingSearch()
        {
            throw new NotImplementedException();
        }

        public async Task<OrganizationDto> SearchByName(string name)
        {
            var organization =  await _organizationRepository.Get(x => x.Name.Equals(name));
            return MapperConfig.GetMapper().Map<OrganizationDto>(organization);
        }

        public async Task<int> Update(UpdateOrganizationDto dto)
        {
            var check = await SearchByName(dto.Name);
            if ((await GetById(dto.Id)) == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            if (check == null || dto.Id == check.Id)
            {
                var organization = MapperConfig.GetMapper().Map<Organization>(dto);
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
