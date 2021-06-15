using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Art_Exchange_Token_System.Entities
{
    public class PendingArtTrade
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<UserData> TradingUsers { get; set; }
        public ICollection<ArtTradeOffer> UserOffers { get; set; }
    }
}