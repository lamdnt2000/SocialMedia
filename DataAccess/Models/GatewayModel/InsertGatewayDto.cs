using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.GatewayModel
{
    public class InsertGatewayDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Type { get; set; }
        public string BankCode { get; set; }
        public string AccessKey { get; set; }
        public string SerectKey { get; set; }
        public int Status { get; set; }
        public int AdminId { get; set; }

    }
}
