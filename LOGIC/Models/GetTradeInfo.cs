using System.Collections.Generic;
using static DATA.Enums.TradeTransactionStatusEnum;

namespace LOGIC.Models
{
    public class GetTradeInfoModel
    {
        public long Id { get; set; }
        public List<UserTradeOfferModel> UserTrades { get; set; }
    }

    public class UserTradeOfferModel
    {
        public string Username { get; set; }
        public TradeStatus TradeSatus { get; set; }
        public List<ArtInfoModel> OfferedArt { get; set; }
    }

    public class ArtInfoModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string FileName { get; set; }        
    }
}
