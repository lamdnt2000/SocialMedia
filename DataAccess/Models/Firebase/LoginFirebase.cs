using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Firebase
{
    public class LoginFirebase
    {
        [Display(Name = "token")]
        [Required(ErrorMessage = "Firebase token must not empty!")]
        public string Token { get; set; }
        public string DeviceToken { get; set; }
    }
}
