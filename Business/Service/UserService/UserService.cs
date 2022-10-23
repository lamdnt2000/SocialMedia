using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Models.LoginUser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Enum;
using User = DataAccess.User;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;

namespace Business.Service.UserService
{
    public class UserService :  IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
                    if (userRecord != null) {
                        if (!userRecord.EmailVerified)
                        {
                            string hashPass = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
                            userDTO.Password = hashPass;
                            user = MapperConfig.GetMapper().Map<User>(userDTO);
                            user.RoleId = (int)EnumConst.RoleEnum.MEMBER;
                            user.Status = (int)EnumConst.UserStatus.NEW;
                            user.CreatedDate = DateTime.Now;
                            return await _userRepository.Insert(user);
                        }
                        else
                        {
                            throw new Exception("Account already verify on firebase");
                        }
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
                else if (user.Status == (int)EnumConst.UserStatus.NEW)
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


        public Task<int> Update(InsertUserDTO user)
        {
            return null;
        }

    }
}
