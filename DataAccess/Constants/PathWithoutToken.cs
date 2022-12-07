using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public class PathWithoutToken
    {
        public const string USER_PATH = "users";
        public const string USER_LOGIN = USER_PATH + "/login";
        public const string USER_SIGNUP = USER_PATH + "/signup";
        public const string HANGFIRE = "/hangfire";
        public const string NOTIFICATION = "/notification";
    }
}
