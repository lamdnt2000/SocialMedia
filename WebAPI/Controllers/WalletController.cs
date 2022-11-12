using Business.Config;
using Business.Constants;
using Business.Service.WalletService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Constant;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using Business.Service.TransactiondepositService;
using DataAccess.Models.TransectionDepositModel;
using DataAccess.Models.WalletModel;

namespace WebAPI.Controllers
{
    [Route(ApiPath.WALLET_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ITransactiondepositService _transactionDepositService;

        public WalletController(IWalletService reactionTypeService, ITransactiondepositService transactionDepositService)
        {
            _walletService = reactionTypeService;
            _transactionDepositService = transactionDepositService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryId(int id)
        {
            try
            {
                var result = await _walletService.GetById(id);
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
        public async Task<IActionResult> GetAll([FromQuery] WalletPaging paging)
        {
            try
            {


                var result = await _walletService.SearchAsync(paging);
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



        [HttpPost()]
        public async Task<IActionResult> InsertPlatformId([FromForm] InsertWalletDto dto)
        {
            try
            {
                var result = await _walletService.Insert(dto);
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
        public async Task<IActionResult> UpdatePlatfrom(int id, [FromForm] UpdateWalletDto dto)
        {
            try
            {
                var result = await _walletService.Update(id, dto);
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
        public async Task<IActionResult> DeletePlatformById(int id)
        {
            try
            {
                var result = await _walletService.Delete(id);
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
        [Route("{id}/transactiondeposits")]
        public async Task<IActionResult> GetCollectionTransactionDeposit(int id)
        {
            try
            {
                var result = await _walletService.GetById(id, true);
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
