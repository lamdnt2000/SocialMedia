using Business.Config;
using Business.Constants;
using Business.Service.PakageService;
using Business.Service.SubscriptionService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Constant;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using DataAccess.Models.PackageModel;
using DataAccess.Models.SubscriptionModel;
using Business.Service.FeatureService;
using DataAccess.Models.FeatureModel;
using DataAccess.Models.PlanModel;
using Business.Service.PlanService;
using System.ComponentModel.DataAnnotations;
using DataAccess.Enum;

namespace WebAPI.Controllers
{
    [Route(ApiPath.PACKAGE_PATH)]
    [ApiController]

    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly IFeatureService _featureService;
        private readonly IPlanService _planService;

        public PackageController(IPackageService packageService
            , IFeatureService featureService
            , IPlanService planService)
        {
            _packageService = packageService;
            _featureService = featureService;
            _planService = planService;

        }

        [HttpGet("{id}")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> GetPackageId(int id)
        {
            try
            {
                var result = await _packageService.GetById(id);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpGet]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> GetAll([FromQuery] PakagePaging paging)
        {
            try
            {


                var result = await _packageService.SearchAsync(paging);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpGet("all")]
        [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
        public async Task<IActionResult> GetAllPackage()
        {
            try
            {
                var result = await _packageService.GetAll();
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }




        [HttpPost]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> InsertPackage([FromForm] InsertPakageDto dto)
        {
            try
            {
                var result = await _packageService.Insert(dto);
                return JsonResponse(201, INSERT_SUCCESS, new { id = result });
            }
            catch (Exception e)
            {

                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, INSERT_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpPut("{id}")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> UpdatePackage(int id, [FromForm] UpdatePakageDto dto)
        {
            try
            {
                var result = await _packageService.Update(id, dto);
                return JsonResponse(200, UPDATE_SUCCESS, new { id = result });
            }
            catch (Exception e)
            {

                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, UPDATE_FAILED, e.Message);
                }
                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, UPDATE_FAILED, e.Message);
                }

                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpDelete("{id}")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> DeletePackageById(int id)
        {
            try
            {
                var result = await _packageService.Delete(id);
                return JsonResponse(200, DELETE_SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, DELETE_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpPost("{id}/features")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> InsertOrUpdateFeatures(int id, [Required][EnumDataType(typeof(EnumFeature))] EnumFeature feature)
        {
            try
            {
                var result = await _featureService.Insert(id, feature);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, INSERT_FAILED, e.Message);
                }
                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, INSERT_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpGet("{id}/features")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> GetFeatures(int id)
        {
            try
            {
                var result = await _packageService.GetPackageInclude(id);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpPut("features/{id}")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> UpdateFeature(int id, [Required][MaxLength(100)] string description)
        {
            try
            {
                var result = await _featureService.Update(id, description);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpGet("features/recommend")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> RecommendFeture()
        {
            try
            {
                var result = _featureService.ValidFeature();
                return JsonResponse(200, SUCCESS, result.Select(x => x.Name));
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpDelete("features/{id}")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            try
            {
                var result = await _featureService.Delete(id);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, DELETE_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpPost("{id}/plans")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> InsertPlan(int id, [Required] string planName)
        {
            try
            {
                var result = await _planService.InsertPlanByName(id, planName);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, INSERT_FAILED, e.Message);
                }
                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, INSERT_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpGet("{id}/plans")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> GetPlans(int id)
        {
            try
            {
                var result = await _packageService.GetPlansOfPackage(id);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }


        [HttpGet("plans/{id}")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> GetPlanById(int id)
        {
            try
            {
                var result = await _planService.GetPlan(id);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpPut("plans/{id}")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> UpdatePlan(int id, [FromBody] InsertPlanDto dto)
        {
            try
            {
                var result = await _planService.UpdatePlan(id, dto);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, UPDATE_FAILED, e.Message);
                }
                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, UPDATE_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpDelete("plans/{id}")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        public async Task<IActionResult> DeletePlan(int id)
        {
            try
            {
                var result = await _planService.Delete(id);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, DELETE_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }
    }
}
