using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WebAPI.Constant;
using static Business.Utils.ResponseFormat;
using API.Service.Authorize;
using Business.Service.UserService;
using DataAccess.Models.LoginUser;
using DataAccess.Models;
using Microsoft.Extensions.Options;
using Business.Config;
using Business.Constants;
using static Business.Constants.ResponseMsg;
using Business.Service.WalletService;
using DataAccess.Models.PaymentModel;
using Business.Utils;
using Firebase.Auth;
using Business.Service.TransactionDepositService;
using DataAccess.Models.TransectionDepositModel;

namespace WebAPI.Controllers
{
    [Route(ApiPath.USER_PATH)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IWalletService _walletService;
        public UserController(IUserService userService, IAuthService authService, 
            IOptions<FirebaseMetadata> firebaseMetadata, 
            IWalletService walletService,
            ITransactionDepositService transactionDepositService)
        {
            _userService = userService;
            _authService = authService;
            _walletService = walletService;

        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromForm] InsertUserDTO dto)
        {
            try
            {
                var result = await _userService.Insert(dto);

                if (result == 1)
                {
                    return JsonResponse(201, INSERT_SUCCESS, "Please verify your email");
                }
                else
                {
                    return JsonResponse(403, INSERT_FAILED, "User create fail try gain");
                }
            }
            catch (Exception e)
            {
                return JsonResponse(400, e.Message, null);
            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userService.GetUser(loginUser);
                _authService.SetCurrentUser(user);
                return JsonResponse(200, "Success", new { Token = _authService.CreateToken() ,FirstName = user.Firstname, LastName = user.Lastname, Role = user.Role.Name, Status = (user.Status==2)?true:false });

            }
            catch (Exception e)
            {
                return JsonResponse(401, "Not authenticate", e.Message);
            }

        }

        [HttpPost]
        [Route("loginGoogle")]
        public async Task<IActionResult> LoginGoogle([FromForm]string TokenId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {
                var user = await _userService.GoogleSignIn(TokenId);
                _authService.SetCurrentUser(user);
                return JsonResponse(200, "Success", new { Token = _authService.CreateToken(), FirstName = user.Firstname, LastName = user.Lastname, Role = "Member" });

            }
            catch (Exception e)
            {
                return JsonResponse(401, "Not authenticate", e.Message);
            }

        }


        [HttpGet]
        [Route("me")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {

                var user = await _userService.GetCurrentUserProfile();
         
                return JsonResponse(200, "Success",user);

            }
            catch (Exception e)
            {
                return JsonResponse(401, "Not authenticate", e.Message);
            }
        }

        [HttpPut]
        [Route("profile")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _userService.UpdateUserInformation(dto);

                return JsonResponse(200, "Success", "");

            }
            catch (Exception e)
            {
                if (e.Message.Contains("Duplicated"))
                {
                    return JsonResponse(400, e.Message,"");
                }
                return JsonResponse(401, "Not authenticate", e.Message);
            }
        }

        [HttpPut]
        [Route("password")]
        [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
        public async Task<IActionResult> UpdateUserPassword([FromForm] UpdateUserPassworDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _userService.UpdateUserPassword(dto);

                return JsonResponse(200, "Success", "");

            }
            catch (Exception e)
            {
                if (e.Message.Contains("password"))
                {
                    return JsonResponse(400, "Bad request", e.Message);
                }
                return JsonResponse(401, "Not authenticate", e.Message);
            }
        }

        [HttpGet("wallet")]
        [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
        public async Task<IActionResult> GetWallet()
        {
            try
            {
                var result = await _walletService.GetCurrentWallet();
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

        [HttpGet("wallet/history")]
        [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
        public async Task<IActionResult> GetHistoryWallet([FromQuery]TransactionDepositPaging paging)
        {
            try
            {
                var result = await _walletService.SearchTransaction(paging);
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


        [HttpPost("wallet")]
        [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
        public async Task<IActionResult> CreateWallet()
        {
            try
            {
                var result = await _walletService.Insert();
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, DUPLICATED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }
        
        [HttpPost("wallet/deposit")]
        [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
        public async Task<IActionResult> DepositMoney(PaymentDto dto)
        {
            try
            {
                var result = await _walletService.CreateDepositLink(dto);
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

        
        [HttpPost("wallet/verify")]
        [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
        public async Task<IActionResult> UpdateBalanceAfterDeposit()
        {
            try
            {
                var vnpayData = Request.Query;
                VnPayLibrary vnpay = new VnPayLibrary();
                foreach (string s in vnpayData.Keys)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                var result = await _walletService.UpdateBalance(vnpay);
                return JsonResponse(200, INSERT_SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                if (e.Message.Contains(INVALID))
                {
                    return JsonResponse(400, INVALID, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }
    }
}
