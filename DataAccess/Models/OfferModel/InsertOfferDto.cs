using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.OfferModel
{
    public class InsertOfferDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Duration { get; set; }
        public bool Status { get; set; }
    }
}
