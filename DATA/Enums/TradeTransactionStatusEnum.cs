using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATA.Enums
{
    public class TradeTransactionStatusEnum
    {
        public enum TradeStatus
        {
            NoAction=0,
            RequestedRevaluation = 1,
            OfferAccepted = 2,
            OfferChange=4,
            NotifyAboutChange=5,
        }
    }
}
