using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Art_Exchange_Token_System.Enums.TradeTransactionStatusEnum;

namespace Art_Exchange_Token_System.Models
{
    public class AllOnGoingTradesModel
    {
        public List<OnGoingTradeModel> OnGoingTrades { get; set; }
    }
    public class OnGoingTradeModel
    {
        public long TradeId { get; set; }
        public List<UserInformationModel> TradingUsers { get; set; }
    }

    public class UserInformationModel
    {
        public string UserName { get; set; }
        public TradeStatus TradeStatus { get; set; }
        public List<TradeInformation> OfferedArt { get; set; }
    }

    public class TradeInformation
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
