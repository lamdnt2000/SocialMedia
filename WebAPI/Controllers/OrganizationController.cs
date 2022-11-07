using Business.Config;
using Business.Constants;
using Business.Repository.OrganizationRepo;
using Business.Service.OrganizationService;
using Business.Service.UserService;
using DataAccess.Models.LoginUser;
using DataAccess.Models.OrganizationModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using DataAccess.Entities;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route(Constant.ApiPath.ORAGANIZATION_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganizationById(int id)
        {
            try
            {

              
                var result = await _organizationService.GetById(id);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {
                
                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, "");
                }
                return JsonResponse(401, UNAUTHORIZE, "");

            }
        }

        [HttpPost()]
        public async Task<IActionResult> InsertOrganizationId([FromForm] InsertOrganizationDto dto)
        {
            try
            {
                var result = await _organizationService.Insert(dto);
                return JsonResponse(200, SUCCESS, new { id = result});
            }
            catch (Exception e)
            {

                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, DUPLICATED, "");
                }
                return JsonResponse(401, UNAUTHORIZE, "");

            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrganizationId([FromForm] UpdateOrganizationDto dto)
        {
            try
            {
                var result = await _organizationService.Update(dto);
                return JsonResponse(200, SUCCESS, new { id = result });
            }
            catch (Exception e)
            {

                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, DUPLICATED, "");
                }
                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, "");
                }
                
                return JsonResponse(401, UNAUTHORIZE, "");

            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganizationById(int id)
        {
            try
            {
                var result = await _organizationService.Delete(id);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, "");
                }
                return JsonResponse(401, UNAUTHORIZE, "");

            }
        }
    }
}
