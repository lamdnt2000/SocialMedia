﻿using DataAccess.Entities;
using System.Threading.Tasks;


namespace API.Service.Authorize
{
    public interface IAuthService
    {
        Task<bool> AuthFirebaseToken(string token);
        string CreateToken();
        User GetCurrentUser();
        void SetCurrentUser(User user);
    }
}
