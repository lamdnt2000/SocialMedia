using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.HashtagModel
{
    public class InsertHashtagDto
    {
        [Required]
        [MaxLength(225)]
        public string Name { get; set; }
        [MaxLength(225)]
        public string Description { get; set; }
        [Required]
        public int PostHashtagId { get; set; }
    }
}
