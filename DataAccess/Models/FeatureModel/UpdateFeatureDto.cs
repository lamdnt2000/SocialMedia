using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.FeatureModel
{
    public class UpdateFeatureDto:InsertFeatureDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
