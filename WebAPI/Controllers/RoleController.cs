using Business.Service.Rule;
using Business.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

using static Business.Utils.ResponseFormat;
using DataAccess.Models.Role;
using Business.Pagination.Model;
using DataAccess;
using WebAPI.Constant;
using Business.Config;
using Business.Constants;

namespace WebAPI.Controllers
{
    [Route(ApiPath.ROLE_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

     

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();

            return JsonResponse(200, ResponseMsg.SUCCESS, roles);
        }
        
        [HttpGet("/demo")]
        public async Task<IActionResult> Paging(PaginatedInputModel paginatedInputModel)
        {
            var roles = new Role();

            return JsonResponse(200, ResponseMsg.SUCCESS, roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] InsertRoleDTO dto)
        {
            try
            {
              
                await _roleService.Insert(dto);
                return JsonResponse(201, ResponseMsg.INSERT_SUCCESS, null);
            }
            catch (Exception e)
            {
                return e.Message.Contains("not found")
                    ? JsonResponse(400, e.Message, null)
                    : ErrorResponse(e.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> UpdateArea(UpdateRoleDTO dto)
        {
            try
            {
            
                await _roleService.Update(dto);
                return JsonResponse(200, ResponseMsg.SUCCESS, null);
            }
            catch (Exception e)
            {
                return e.Message.Contains("not found")
                    ? JsonResponse(400, e.Message, null)
                    : ErrorResponse(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                await _roleService.Delete(id);
                return JsonResponse(200, ResponseMsg.SUCCESS, null);
            }
            catch (Exception e)
            {
                return e.Message.Contains("not found")
                    ? JsonResponse(400, e.Message, null)
                    : ErrorResponse(e.Message);
            }
        }
    }
}
