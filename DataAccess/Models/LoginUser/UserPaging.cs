using AutoFilterer.Attributes;
using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.LoginUser
{
    public class UserPaging: PaginationFilterBase
    {
        [ToLowerContainsComparison]
        public string LastName { get; set; }
        
    }
}
