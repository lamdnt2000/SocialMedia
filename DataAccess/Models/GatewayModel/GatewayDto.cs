using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.GatewayModel
{
    public class GatewayDto:InsertGatewayDto
    {
        public int Id { get; set; }

        public virtual ICollection<TransactionDepositsDto> TransactionDeposits { get; set; }
    }
}
