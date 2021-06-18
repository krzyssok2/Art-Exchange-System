﻿using System.Collections.Generic;
using static DATA.Enums.TradeTransactionStatusEnum;

namespace LOGIC.Models
{
    public class GetAllTradesTransaction
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public AllOnGoingTradesModel AllOngoingTrades { get; set; }
    }

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
