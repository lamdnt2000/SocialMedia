using Business.Config;
using Business.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Constant;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using Business.Service.GatewayService;
using DataAccess.Models.GatewayModel;
using Business.Service.TransactiondepositService;
using DataAccess.Models.TransectionDepositModel;

namespace WebAPI.Controllers
{
    [Route(ApiPath.GATEWAY_PATH)]
    [ApiController]
   
    public class GatewayController : ControllerBase
    {
        private readonly IGatewayService _gatewayService;
        private readonly ITransactiondepositService _transactionDepositService;
        public GatewayController(IGatewayService gatewayService, ITransactiondepositService transactionDepositService)
        {
            _gatewayService = gatewayService;
            _transactionDepositService = transactionDepositService;
        }

        [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGatewayById(int id)
        {
            try
            {


                var result = await _gatewayService.GetById(id);
                if (result != null)
                {
                    result.TransactionDeposits = null;
                    return JsonResponse(200, SUCCESS, result);
                }
                else
                {
                    return JsonResponse(400, NOT_FOUND, result);
                }
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

        [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GatewayPaging paging)
        {
            try
            {


                var result = await _gatewayService.SearchAsync(paging);
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

        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        [HttpGet("transactiondeposits")]
        public async Task<IActionResult> GetAllTransactionDeposit([FromQuery] TransactionDepositPaging paging)
        {
            try
            {


                var result = await _transactionDepositService.SearchAsync(paging);
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


        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        [HttpPost()]
        public async Task<IActionResult> InsertPlatformId([FromForm] InsertGatewayDto dto)
        {
            try
            {
                var result = await _gatewayService.Insert(dto);
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

        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlatfrom(int id, [FromForm] UpdateGatewayDto dto)
        {
            try
            {
                var result = await _gatewayService.Update(id, dto);
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

        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlatformById(int id)
        {
            try
            {
                var result = await _gatewayService.Delete(id);
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


        [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
        [HttpGet]
        [Route("{id}/transactiondeposits")]
        public async Task<IActionResult> GetCollectionTransactionDeposit(int id)
        {
            try
            {
                var result = await _gatewayService.GetById(id, true);
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
