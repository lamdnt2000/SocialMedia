using DataAccess.Models.BranModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.OrganizationModel
{
    public class UpdateOrganizationDto : InsertOrganizationDto
    {
        [Required]
        public bool Status { get; set; }

    }
}
