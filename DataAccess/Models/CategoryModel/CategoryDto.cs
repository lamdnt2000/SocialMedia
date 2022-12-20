using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.CategoryModel
{
    public class CategoryDto:InsertCategoryDto
    {
        public long Id { get; set; }
        public int TotalChannel { get; set; } = 0;
    }
}
