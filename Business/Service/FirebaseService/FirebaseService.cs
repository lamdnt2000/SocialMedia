using DataAccess.Models;
using Firebase.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace API.Service.FirebaseService
{
    public class FirebaseService : IFirebaseService
    {
        
        private readonly FirebaseMetadata _firebaseMetadata;

        public FirebaseService(IOptions<FirebaseMetadata> firebaseMetadata)
        {

            _firebaseMetadata = firebaseMetadata.Value;
        }

        public async Task<bool> SendVerifyEmail(string email, string password)
        {
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseMetadata.ApiKey));
                var user = await auth.SignInWithEmailAndPasswordAsync(email, password);
                await auth.SendEmailVerificationAsync(user.FirebaseToken);

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
