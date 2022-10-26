using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.LoginUser
{
    public class FacebookLoginDto
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string TokenId { get; set; }
    }
}
