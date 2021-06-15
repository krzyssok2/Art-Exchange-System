﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static Art_Exchange_Token_System.Enums.TradeTransactionStatusEnum;

namespace Art_Exchange_Token_System.Entities
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
