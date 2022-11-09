using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.BranModel
{
    public class UpdateBrandDto:InsertBrandDto
    {
        [Required]
        public bool Status { get; set; }
    }
}
