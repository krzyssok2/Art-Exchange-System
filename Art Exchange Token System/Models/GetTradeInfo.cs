using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Art_Exchange_Token_System.Enums.TradeTransactionStatusEnum;

namespace Art_Exchange_Token_System.Models
{
    public class GetTradeInfoModel
    {
        public long Id { get; set; }
        public List<UserTradeOfferModel> UserTrades { get; set; }
    }

    public class UserTradeOfferModel
    {
        public string Username { get; set; }
        public TradeStatus tradeStatus { get; set; }
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
