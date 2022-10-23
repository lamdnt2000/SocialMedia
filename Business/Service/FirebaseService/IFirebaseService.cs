using System.Threading.Tasks;

namespace API.Service.FirebaseService
{
    public interface IFirebaseService
    {
        Task<bool> SendVerifyEmail(string email, string password);
    }
}
