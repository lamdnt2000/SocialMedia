using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business.Service.UserService;
using DataAccess.Entities;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static DataAccess.Enum.EnumConst;

namespace API.Service.Authorize
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private User _user;

        public AuthService(
            IUserService userService,
            IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public string CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims();
            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:Lifetime"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );

            return token;
        }

        private List<Claim> GetClaims()
        {
            RoleEnum role = (RoleEnum)_user.RoleId;

            var claims = new List<Claim>
            {
                new Claim("id", _user.Id.ToString()),
                new Claim("email", _user.Email),
                new Claim("name", _user.Firstname+" "+_user.Lastname),
                new Claim("role", role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            };

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = _configuration["Jwt:Secret"];
            var secret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        //validate token from header
        public static SecurityToken ValidateJSONWebToken(string token, IConfiguration configuration)
        {
            var handler = new JwtSecurityTokenHandler();
            //lay secret tu appsetting.json
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]));
            //check token voi security
            try
            {
                ClaimsPrincipal claims = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    IssuerSigningKey = securityKey,
                }, out SecurityToken validatedToken);

                return validatedToken;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> AuthFirebaseToken(string token)
        {
            try
            {
                var tokenDecoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                var userFirebase = await FirebaseAuth.DefaultInstance.GetUserAsync(tokenDecoded.Uid);
                var user = await _userService.FindByEmail(userFirebase.Email);
                if (user != null)
                {
                    _user = user;
                    return true;
                }
                return false;
            }
            catch (FirebaseAuthException)
            {
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public User GetCurrentUser()
        {
            return _user;
        }

        public void SetCurrentUser(User user)
        {
            _user=user;
        }
    }
}
