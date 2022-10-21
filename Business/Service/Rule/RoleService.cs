using Business.Repository.RoleRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.Role;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Service.Rule
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                Role role = await _roleRepository.Get(x => x.Id == id);
                if (role == null)
                {
                    throw new Exception(role.GetType().Name + " Not found");
                }
                return await _roleRepository.Delete(id);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<RoleDTO>> GetAllAsync()
        {
            try
            {
                var roles = await _roleRepository.GetAllAsync();
                return MapperConfig.GetMapper().Map<List<RoleDTO>>(roles);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> Insert(InsertRoleDTO dto)
        {
            try
            {
                Role role = MapperConfig.GetMapper().Map<Role>(dto);
                return await _roleRepository.Insert(role);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> Update(UpdateRoleDTO dto)
        {
            try
            {
                Role role = await _roleRepository.Get(x => x.Id == dto.Id);
                if (role == null)
                {
                    throw new Exception(role.GetType().Name + " Not found");
                }
                role = MapperConfig.GetMapper().Map<Role>(dto);
                return await _roleRepository.Update(role);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
