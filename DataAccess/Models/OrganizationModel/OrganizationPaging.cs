using AutoFilterer.Attributes;
using AutoFilterer.Types;

namespace DataAccess.Models.OrganizationModel
{
    public class OrganizationPaging: PaginationFilterBase
    {
        [ToLowerContainsComparison]
        public string Name { get; set; }
       
        
    }
}
