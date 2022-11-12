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

namespace WebAPI.Controllers
{
    [Route(ApiPath.PACKAGE_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
    public class PackageController : ControllerBase
    {
        private readonly IPakageService _pakageService;
        private readonly ISubscriptionService _subscriptionService;

        public PackageController(IPakageService pakageService, ISubscriptionService subscriptionService)
        {
            _pakageService = pakageService;
            _subscriptionService = subscriptionService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageId(int id)
        {
            try
            {
                var result = await _pakageService.GetById(id);
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
        public async Task<IActionResult> GetAll([FromQuery] PakagePaging paging)
        {
            try
            {


                var result = await _pakageService.SearchAsync(paging);
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


        [HttpGet("Subscriptions")]
        public async Task<IActionResult> GetAllSubscription([FromQuery] SubscriptionPaging paging)
        {
            try
            {


                var result = await _subscriptionService.SearchAsync(paging);
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

        [HttpPost()]
        public async Task<IActionResult> InsertPackageId([FromForm] InsertPakageDto dto)
        {
            try
            {
                var result = await _pakageService.Insert(dto);
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
        public async Task<IActionResult> UpdatePackage(int id, [FromForm] UpdatePakageDto dto)
        {
            try
            {
                var result = await _pakageService.Update(id, dto);
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
        public async Task<IActionResult> DeletePackageById(int id)
        {
            try
            {
                var result = await _pakageService.Delete(id);
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

        [HttpGet]
        [Route("{id}/Subscritptions")]
        public async Task<IActionResult> GetCollectionSubscritption(int id)
        {
            try
            {
                var result = await _pakageService.GetById(id);
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
