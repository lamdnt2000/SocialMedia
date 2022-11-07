using DataAccess.Models.OrganizationModel;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.BranModel
{
    public partial class BrandDto : InsertBrandDto
    {
        [Required]
        public int Id { get; set; }
       
        public InsertOrganizationDto Organization;

        
    }
}
