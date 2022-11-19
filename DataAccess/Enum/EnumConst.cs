using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Enum
{
    public class EnumConst
    {
        public enum RoleEnum
        {
            ADMIN = 1,
            MEMBER = 2,
            BOT = 3,
        }

        public enum UserStatus
        {
            NEW = 1,
            VERIFY = 2,
            BAN = 3
        }

         public enum PlatFormEnum
        {
            YOUTUBE = 1,
            FACEBOOK = 2,
            TIKTOK = 3
        }
        
    }
}
