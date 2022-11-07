using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.PlatFormModel
{
    public class UpdatePlatformDto : InsertPlatformDto
    {
      
        [Required]
        public bool Status { get; set; }

    }
}
