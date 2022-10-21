using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Role
{
    public class InsertRoleDTO
    {
        [Required(ErrorMessage = "Name can't empty!")]
        public string Name { get; set; }
    }
}
