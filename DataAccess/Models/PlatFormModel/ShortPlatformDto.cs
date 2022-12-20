using DataAccess.Models.CategoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.PlatFormModel
{
    public class ShortPlatformDto : InsertPlatformDto
    {
        public int Id { get; set; }
        public bool Status { get; set; }

    }
}
