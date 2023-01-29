using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Models.LoginUser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Enum;
using User = DataAccess.Entities.User;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using DataAccess.Repository.UserTypeRepo;
using DataAccess.Entities;
using DataAccess.Repository.PlanRepo;
using static Business.Constants.ResponseMsg;
namespace Business.Service.UserService
{
    public class UserService : BaseService, IUserService
    {

        private readonly IPlanRepository _planRepository;
        private readonly int FREE_TRIAL_PLAN_ID = 49;
        public UserService(
            IHttpContextAccessor httpContextAccessor
            , IUserRepository userRepository
            , IUserTypeRepository userTypeRepository
            , IPlanRepository planRepository) : 
            base(httpContextAccessor, userRepository, userTypeRepository)
        {
            _planRepository = planRepository;  
        }

        public bool CheckPassword(string inputPassword, string currentPassword)
        {
            bool isValid = BCrypt.Net.BCrypt.Verify(inputPassword, currentPassword);

            return isValid;
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _userRepository.Get(x => x.Email == email && x.Status != (int)EnumConst.UserStatus.BAN);
        }

        public async Task<int> Insert(InsertUserDTO userDTO)
        {
            try
            {
                // create user to firebase
                User user = await FindByEmail(userDTO.Email);
                if (user == null)
                {
                    UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(userDTO.Email);
                    if (userRecord != null)
                    {
                        string hashPass = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
                        user = MapperConfig.GetMapper().Map<User>(userDTO);
                        user.RoleId = (int)EnumConst.RoleEnum.MEMBER;
                        user.Status = (userRecord.EmailVerified)?(int)EnumConst.UserStatus.VERIFY: (int)EnumConst.UserStatus.NEW;
                        user.CreatedDate = DateTime.Now;
                        user.Provider = userRecord.ProviderId;
                        user.Password = hashPass;
                        var userType = new UserType()
                        {
                            DateStart = DateTime.Now,
                            DateEnd = DateTime.Now.AddMonths(1),
                            Name = "Free Trial",
                            Valid = true,
                            Feature = await GetFreeTrialFeature(),
                        };
                        user.UserType = userType;
                        return await _userRepository.Insert(user);
                    }
                    else
                    {
                        throw new Exception("Something wrong in signup account try again");
                    }
                }
                else
                {
                    throw new Exception("Duplicated email on server. Try orthers");
                }

            } 
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<User> GetUser(LoginUserDTO userDTO)
        {
            try
            {

                var user = await _userRepository.Get(x => x.Email == userDTO.Email && x.Status != (int)EnumConst.UserStatus.BAN, new List<String>{ "Role"});
                if (user == null)
                {
                    throw new Exception("User Not Found");
                }
                if (!user.Provider.Equals("firebase"))
                {
                    throw new Exception("Your user auth not support");
                }
                if (!CheckPassword(userDTO.Password, user.Password))
                {
                    throw new Exception("Invalid user or password");
                }

                if (user.Status == (int)EnumConst.UserStatus.NEW)
                {
                    UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(userDTO.Email);
                    if (userRecord != null && userRecord.EmailVerified)
                    {
                        user.Status = (int)EnumConst.UserStatus.VERIFY;
                        await _userRepository.Update(user);        
                    }
                    return user;

                }
                else
                {
                    return user;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }



        public async Task<User> GoogleSignIn(InsertUserDTO dto)
        {
            FirebaseToken firebaseToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(dto.TokenId);
            var claims = firebaseToken.Claims;
            string email = (string)claims.GetValueOrDefault("email");
            User user = await _userRepository.Get(x => x.Email == dto.Email && x.Status != (int)EnumConst.UserStatus.BAN, new List<String> { "Role" });
            if (user == null)
            {
                user = MapperConfig.GetMapper().Map<User>(dto);
                user.Email = email;
                user.CreatedDate = DateTime.Now;
                user.Firstname = dto.Firstname;
                user.Lastname = dto.Lastname;
                user.Status = (int)EnumConst.UserStatus.VERIFY;
                user.RoleId = (int)EnumConst.RoleEnum.MEMBER;
                user.Provider = "google.com";
                await _userRepository.Update(user);
            }
            return user;
        }

        /*public async Task<User> FacebookSignIn(string TokenId)
        {
            FirebaseToken firebaseToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(TokenId);
            var claims = firebaseToken.Claims;
            string email = (string)claims.GetValueOrDefault("email");
            if (email == null)
            {
                throw new Exception("User not found");
            }
            else
            {
                User user = await FindByEmail(email);
                if (user != null)
                {
                    user = new User();
                    await _userRepository.Update(user);
                    return user;
                }
                else
                {
                    throw new Exception("User not found");
                }
            }


        }*/

        public async Task<User> GoogleSignUp(GoogleSignUpDto dto)
        {
            FirebaseToken firebaseToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(dto.TokenId);
            var claims = firebaseToken.Claims;
            string email = (string)claims.GetValueOrDefault("email");
            User user = await FindByEmail(email);
            if (user == null)
            {
                
            }
            return user;
        }

       /* public async Task<User> FacebookSignUp(FacebookSignUpDto dto)
        {
            FirebaseToken firebaseToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(dto.TokenId);
            User user = await FindByEmail(dto.Email);
            if (user == null)
            {
                user = MapperConfig.GetMapper().Map<User>(dto);
                user.CreatedDate = DateTime.Now;
                user.Status = (int)EnumConst.UserStatus.VERIFY;
                user.RoleId = (int)EnumConst.RoleEnum.MEMBER;
                user.Provider = "facebook.com";
                await _userRepository.Update(user);
                var userRecords = new UserRecordArgs
                {
                    Email = dto.Email,
                    EmailVerified = true,
                    DisplayName = dto.FirstName + " " + dto.LastName,
                    Uid = firebaseToken.Uid
                };
                await FirebaseAuth.DefaultInstance.UpdateUserAsync(userRecords);
            }
            return user;
        }*/

        public async Task<User> GoogleSignIn(string TokenId)
        {
            FirebaseToken firebaseToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(TokenId);
            var claims = firebaseToken.Claims;
            string email = (string)claims.GetValueOrDefault("email");
            
            User user = await FindByEmail(email);
            if (user == null)
            {
                string name = ((string)claims.GetValueOrDefault("name"));
                int lastSpace = name.LastIndexOf(" ");
                user = new User();
                user.Email = email;
                user.Firstname = name.Substring(0,lastSpace);
                user.Lastname = name.Substring(lastSpace+1);
                user.Status = (int)EnumConst.UserStatus.VERIFY;
                user.RoleId = (int)EnumConst.RoleEnum.MEMBER;
                user.Provider = "google.com";
                var userType = new UserType()
                {
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now.AddMonths(1),
                    Name = "Free Trial",
                    Valid = true,
                    Feature = await GetFreeTrialFeature(),
                };
                user.UserType = userType;
                await _userRepository.Insert(user);
            }
            return user;
        }

        public async Task<bool> UpdateUserInformation(UpdateUserDto dto)
        {
            var currentUser = await GetCurrentUser();
            var user = await FindByEmail(dto.Email);
            if (user!=null && user.Id == currentUser.Id)
            {
                currentUser.Email = dto.Email;
                currentUser.Firstname = dto.Firstname;
                currentUser.Lastname = dto.Lastname;
                currentUser.Username = dto.UserName;
                if (dto.Phone != null)
                {
                    currentUser.Phone = dto.Phone;
                }
                int result = await _userRepository.Update(currentUser);
                return (result > 0) ? true : false;
            }
            else
            {
                throw new Exception("Duplicated email");
            }

        }

        public async Task<bool> UpdateUserPassword(UpdateUserPassworDto dto)
        {
            var currentUser = await GetCurrentUser();
            if (!dto.Password.Equals(dto.ConfirmPassword))
            {
                throw new Exception("Confirm Password " + NOT_MATCH);
            }
            if (CheckPassword(dto.CurrentPassword, currentUser.Password))
            {
                currentUser.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                var result = await _userRepository.Update(currentUser);
                return (result > 0) ? true : false;

            }
            else
            {
                throw new Exception("Old password "+NOT_MATCH);
            }
        }

        public async Task<ProfileDto> GetCurrentUserProfile()
        {
            var user = await GetCurrentUser();
            
            return MapperConfig.GetMapper().Map<ProfileDto>(user);
        }

        private async Task<string> GetFreeTrialFeature()
        {
            return await _planRepository.ConvertPlanFeatureToJson(FREE_TRIAL_PLAN_ID);
        }
    }
}
