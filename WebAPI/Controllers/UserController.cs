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
                return JsonResponse(200, "Success", new { Token = _authService.CreateToken() });

            }
            catch (Exception e)
            {
                return JsonResponse(401, "Not authenticate", e.Message);
            }

        }

        [HttpPost]
        [Route("loginGoogle")]
        public async Task<IActionResult> LoginGoogle([FromForm] GoogleLoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {

                var user = await _userService.GoogleSignIn(dto);
                _authService.SetCurrentUser(user);
                return JsonResponse(200, "Success", new { Token = _authService.CreateToken() });

            }
            catch (Exception e)
            {
                return JsonResponse(401, "Not authenticate", e.Message);
            }

        }

        [HttpPost]
        [Route("loginFacebook")]
        public async Task<IActionResult> LoginFacebook([FromForm] FacebookLoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var user = await _userService.FacebookSignIn(dto);
                _authService.SetCurrentUser(user);
                return JsonResponse(200, "Success", new { Token = _authService.CreateToken() });

            }
            catch (Exception e)
            {
                return JsonResponse(401, "Not authenticate", e.Message);
            }

        }

        [HttpPost]
        [Route("signupGoogle")]
        public async Task<IActionResult> SignUpGoogle([FromForm] GoogleSignUpDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {

                var user = await _userService.GoogleSignUp(dto);
                _authService.SetCurrentUser(user);
                return JsonResponse(200, "Success", new { Token = _authService.CreateToken() });

            }
            catch (Exception e)
            {
                return JsonResponse(401, "Not authenticate", e.Message);
            }

        }

        [HttpPost]
        [Route("signupFacebook")]
        public async Task<IActionResult> SignUpFacebook([FromForm] FacebookSignUpDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var user = await _userService.FacebookSignUp(dto);
                _authService.SetCurrentUser(user);
                return JsonResponse(200, "Success", new { Token = _authService.CreateToken() });

            }
            catch (Exception e)
            {
                return JsonResponse(401, "Not authenticate", e.Message);
            }

        }

        [HttpGet]
        [Route("me")]
        [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
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
        [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
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
        [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
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
    }
}
