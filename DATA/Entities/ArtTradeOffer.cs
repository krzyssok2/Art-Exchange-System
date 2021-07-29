using System.Collections.Generic;
using static DATA.Enums.TradeTransactionStatusEnum;

namespace DATA.Entities
{
    public class ArtTradeOffer
    {
        public long Id { get; set; }
        public UserData User { get; set; }
        public ICollection<ArtData> OferredArtDatas { get; set; }
        public TradeStatus TradeStatus { get; set; }
        public PendingArtTrade PendingArtTrade { get; set; }
    }
}
