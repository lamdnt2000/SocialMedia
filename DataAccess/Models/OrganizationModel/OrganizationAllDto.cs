using DataAccess.Entities;
using DataAccess.Models.BranModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Models.OrganizationModel
{
    public class OrganizationAllDto:InsertOrganizationDto
    {
        [Required]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Status { get; set; }
        public ICollection<BrandDto> Brands { get; set; }
        public int TotalChannels { get; set; } = 0;
    }
}
