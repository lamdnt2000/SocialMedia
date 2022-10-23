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
using Role = Business.Constants.RoleAuthorize;

namespace WebAPI.Controllers
{
    [Route(ApiPath.USER_PATH)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService, IOptions<FirebaseMetadata> firebaseMetadata)
        {
            _userService = userService;
            _authService = authService;

        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(InsertUserDTO dto)
        {
            try
            {
                var result = await _userService.Insert(dto);

                if (result == 1)
                {
                    return JsonResponse(200, "Please verify your email", "");
                }
                else
                {
                    return JsonResponse(403, "User create fail try gain", "");
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
                return JsonResponse(200, "Success", new { Token = _authService.CreateToken() });

            }
            catch (Exception e)
            {
                return JsonResponse(403, "Not authenticate", e.Message);
            }

        }




    }
}
