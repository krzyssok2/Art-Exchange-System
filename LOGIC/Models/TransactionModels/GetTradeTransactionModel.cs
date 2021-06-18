using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC.Models.TransactionModels
{
    public class GetTradeTransactionModel
    {
        public ErrorHandlingModel ErrorHandling { get; set; }
        public GetTradeInfoModel TradeInfo { get; set; }
    }
}
