using Business.Config;
using Business.Constants;
using Business.Service.OrganizationService;
using DataAccess.Models.OrganizationModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Business.Service.BrandService;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using DataAccess.Models.BranModel;

namespace WebAPI.Controllers
{
    [Route(Constant.ApiPath.BRAND_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandId(int id)
        {
            try
            {
                var result = await _brandService.GetById(id);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, "");
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpPost()]
        public async Task<IActionResult> InsertBrandId([FromForm] InsertBrandDto dto)
        {
            try
            {
                var result = await _brandService.Insert(dto);
                return JsonResponse(200, SUCCESS, new { id = result });
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
        public async Task<IActionResult> UpdateOrganizationId([FromForm] UpdateBrandDto dto)
        {
            try
            {
                var result = await _brandService.Update(dto);
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
        public async Task<IActionResult> DeleteBrandById(int id)
        {
            try
            {
                var result = await _brandService.Delete(id);
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
