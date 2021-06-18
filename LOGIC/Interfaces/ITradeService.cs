using LOGIC.Models;
using LOGIC.Models.ErrorHandlingModels;
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
        public Task<ServiceResponseModel<AllOnGoingTradesModel>> GetAllUserTradesByEmail(string email);
        public Task<ServiceResponseModel> DeleteTradeByIdAsync(string email, long id);
        public Task<ServiceResponseModel<GetTradeInfoModel>> GetTradeInfoById(string email, long id);
        public Task<ServiceResponseModel> ChangeTradeStatusAsync(string email, long id, TradeStatus tradeStatus);
        public Task<ServiceResponseModel<GetTradeInfoModel>> PostNewTradeAsync(string email, TradeCreationModel tradeCreationModel);
    }
}
