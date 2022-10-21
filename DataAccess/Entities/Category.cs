using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PlatformId { get; set; }

        public virtual Platform Platform { get; set; }
    }
}
