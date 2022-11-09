using DataAccess.Models.CategoryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.PlatFormModel
{
    public class PlatformDto:InsertPlatformDto
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<CategoryDto> Categories { get; set; }
    }
}
