using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.PlatFormModel
{
    public class InsertPlatformDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
