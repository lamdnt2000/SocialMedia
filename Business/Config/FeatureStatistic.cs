using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Config
{
    public class FeatureStatistic
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public int Quota { get; set; }
        public bool Valid { get; set; }
    }
}
