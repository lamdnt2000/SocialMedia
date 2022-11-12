using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.PackageModel
{
    public class UpdatePakageDto : InsertPakageDto
    {
        public bool Status { get; set; }
    }
}
