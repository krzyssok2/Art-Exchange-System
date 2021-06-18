using System;
using System.Collections.Generic;

namespace DATA.Entities
{


    public class PendingArtTrade
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<UserData> TradingUsers { get; set; }
        public ICollection<ArtTradeOffer> UserOffers { get; set; }
    }
}