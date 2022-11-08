using DataAccess.Models.OrganizationModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.BranModel
{
    public partial class BrandDto : InsertBrandDto
    {
        [Required]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Status { get; set; }

    }
}
