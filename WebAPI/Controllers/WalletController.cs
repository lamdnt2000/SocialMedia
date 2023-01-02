using Business.Config;
using Business.Constants;
using Business.Service.WalletService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebAPI.Constant;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;

namespace WebAPI.Controllers
{
    [Route(ApiPath.WALLET_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        
        public WalletController(IWalletService reactionTypeService)
        {
            _walletService = reactionTypeService;
        }

        [HttpPut("userId")]
        public async Task<IActionResult> DisableWallet(int userId)
        {
            try
            {
                var result = await _walletService.DisableWallet(userId);
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

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteWallet(int userId)
        {
            try
            {
                var result = await _walletService.Delete(userId);
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

       


    }
}
