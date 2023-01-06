using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.FeatureModel
{
    public class FeatureDto:InsertFeatureDto
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
}
