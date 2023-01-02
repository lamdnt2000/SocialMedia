using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table("watchlist")]
    public partial class Watchlist
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        [Key]
        [Column("channel_id")]
        public int ChannelId { get; set; }

        [ForeignKey(nameof(ChannelId))]
        [InverseProperty(nameof(ChannelCrawl.Watchlists))]
        public virtual ChannelCrawl Channel { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Watchlists")]
        public virtual User User { get; set; }
    }
}
