using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.LoginUser
{
    public class LoginUserDTO
    {
        [Required]
        
        public string Email { get; set; }
        [Required]
        [MinLength(5), MaxLength(35)]
        public string Password { get; set; }
     
        public string TokenId { get; set; }
    }
}
