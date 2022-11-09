using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.ReactionTypeModel
{
    public class ReactionTypePaging: PaginationFilterBase
    {
        public int PlatformId { get; set; }
    }
}
