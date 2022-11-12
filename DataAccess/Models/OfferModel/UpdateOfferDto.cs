using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.OfferModel
{
    public class UpdateOfferDto: InsertOfferDto
    {
        public bool Status { get; set; }
    }
}
