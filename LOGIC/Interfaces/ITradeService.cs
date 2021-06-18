using LOGIC.Models;
using LOGIC.Models.TransactionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DATA.Enums.TradeTransactionStatusEnum;

namespace LOGIC.Interfaces
{
    public interface ITradeService
    {
        public AllOnGoingTradesModel GetAllUserTradesByEmail(string email);
        public Task<ErrorHandlingModel> DeleteTradeByIdAsync(string email, long id);
        public GetTradeTransactionModel GetTradeInfoById(string email, long id);
        public Task<ErrorHandlingModel> ChangeTradeStatusAsync(string email, long id, TradeStatus tradeStatus);
        public Task<PostArtTransactionResult> PostNewTradeAsync(string email, TradeCreationModel tradeCreationModel);
    }
}
