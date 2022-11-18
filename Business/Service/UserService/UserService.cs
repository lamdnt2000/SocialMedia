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

namespace Business.Service.UserService
{
    public class UserService : BaseService, IUserService
    {


        public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository) : base(httpContextAccessor, userRepository)
        {

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
                        user.Status = (userRecord.EmailVerified)?(int)EnumConst.UserStatus.NEW: (int)EnumConst.UserStatus.VERIFY;
                        user.CreatedDate = DateTime.Now;
                        user.Provider = userRecord.ProviderId;
                        user.Password = hashPass;
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

                var user = await FindByEmail(userDTO.Email);
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
                        return user;
                    }
                    else
                    {
                        throw new Exception("User not verify yet. Please verify your email");
                    }
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
            User user = await FindByEmail(email);
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

        public async Task<User> FacebookSignIn(FacebookLoginDto dto)
        {
            FirebaseToken firebaseToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(dto.TokenId);
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
                    user = MapperConfig.GetMapper().Map<User>(dto);
                    user.AccessToken = dto.AccessToken;
                    user.UpdateDate = DateTime.Now;
                    await _userRepository.Update(user);
                    return user;
                }
                else
                {
                    throw new Exception("User not found");
                }
            }


        }

        public async Task<User> GoogleSignUp(GoogleSignUpDto dto)
        {
            FirebaseToken firebaseToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(dto.TokenId);
            var claims = firebaseToken.Claims;
            string email = (string)claims.GetValueOrDefault("email");
            User user = await FindByEmail(email);
            if (user == null)
            {
                user = MapperConfig.GetMapper().Map<User>(dto);
                user.Email = email;
                user.CreatedDate = DateTime.Now;
                user.Status = (int)EnumConst.UserStatus.VERIFY;
                user.RoleId = (int)EnumConst.RoleEnum.MEMBER;
                user.Provider = "google.com";
                await _userRepository.Update(user);
            }
            return user;
        }

        public async Task<User> FacebookSignUp(FacebookSignUpDto dto)
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
        }

        public async Task<User> GoogleSignIn(GoogleLoginDto dto)
        {
            FirebaseToken firebaseToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(dto.TokenId);
            var claims = firebaseToken.Claims;
            string email = (string)claims.GetValueOrDefault("email");
            User user = await FindByEmail(email);
            if (user == null)
            {
                throw new Exception("Your user not found !");
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
                currentUser.Phone = dto.Phone;
                currentUser.UpdateDate = DateTime.Now;
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
            if (CheckPassword(dto.CurrentPassword, currentUser.Password))
            {
                currentUser.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                currentUser.UpdateDate = DateTime.Now;
                var result = await _userRepository.Update(currentUser);
                return (result > 0) ? true : false;

            }
            else
            {
                throw new Exception("Old password not match");
            }
        }

        public async Task<UpdateUserDto> GetCurrentUserProfile()
        {
            var user = await GetCurrentUser();
            return MapperConfig.GetMapper().Map<UpdateUserDto>(user);
        }
    }
}
