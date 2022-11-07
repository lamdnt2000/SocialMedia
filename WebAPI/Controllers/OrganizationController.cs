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
using Business.Service.BrandService;
using DataAccess.Models.BranModel;
using WebAPI.Constant;

namespace WebAPI.Controllers
{
    [Route(ApiPath.ORAGANIZATION_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly IBrandService _brandService;
        
        public OrganizationController(IOrganizationService organizationService, IBrandService brandService)
        {
            _organizationService = organizationService;
            _brandService = brandService;
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrganizationId(int id, [FromForm] UpdateOrganizationDto dto)
        {
            try
            {
                var result = await _organizationService.Update(id, dto);
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

        

        [HttpGet]
        [Route("{id:int}/brands")]
        public async Task<IActionResult> GetCollectionBrand(int id)
        {
            try
            {


                var result = await _organizationService.GetById(id, true);
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
