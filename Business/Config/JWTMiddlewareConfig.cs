using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Service.Authorize;
using API.Utils;
using Business.Constants;

namespace Business.Config
{
    public class JWTMiddlewareConfig
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JWTMiddlewareConfig(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            //get token from header
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                try
                {
                    AttachAccountToContext(context, token);
                    await _next(context);
                    return;
                }
                catch (Exception e)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { StatusCode = StatusCodes.Status400BadRequest, e.Message });
                }
            }
            //check if current user have permission to access this resources
            string[] specialPathWithoutToken = {
                PathWithoutToken.USER_LOGIN,
                PathWithoutToken.USER_SIGNUP,
            };
            string[] regexSpecification = { "", "", "" };
            int index = 0;
            //current path
            string api = context.Request.Path.ToString();
            Regex rgx;
            string path = specialPathWithoutToken.FirstOrDefault(value =>
            {
                rgx = new Regex($"^/.*/{value}/?{regexSpecification[index]}");
                bool isMatch = rgx.IsMatch(api);
                index++;
                if (isMatch)
                {
                    if (value.Contains("file"))
                    {
                        if (!context.Request.Method.Equals("GET")) return false;
                    }
                    return true;
                }

                return false;
            });
            if (api.StartsWith(PathWithoutToken.HANGFIRE) || !string.IsNullOrEmpty(path))
            {
                await _next(context);
                return;
            }
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { StatusCode = StatusCodes.Status401Unauthorized, Message = "Unauthorized" });
        }

        private void AttachAccountToContext(HttpContext context, string token)
        {
            try
            {
                SecurityToken validatedToken = AuthService.ValidateJSONWebToken(token, _configuration);
                var jwt = (JwtSecurityToken)validatedToken;

                var expiredTimeString = jwt.Claims.First(x => x.Type == "exp").Value;
                DateTime date = DateUtil.TimeStampToDateTime(long.Parse(expiredTimeString));

                TimeSpan expiredTime = date - DateTime.Now;

                bool expired = expiredTime.TotalMinutes < 0;
                if (expired) throw new Exception("Invalid token");

                var accountId = jwt.Claims.First(x => x.Type == "id").Value;
                var email = jwt.Claims.First(x => x.Type == "email").Value;
                var role = jwt.Claims.First(x => x.Type == "role").Value;
                context.Items["User"] = new { id = accountId, email = email, role = role };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
