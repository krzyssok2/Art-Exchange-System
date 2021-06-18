using DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DATA.Enums.TradeTransactionStatusEnum;

namespace DATA.Interfaces
{
    public interface ITradeFunctions
    {
        public UserData GetUserDataDetailedArtOffers(string email);
        public PendingArtTrade GetPendingArtTradeById(long id);
        public Task DeleteTrade(PendingArtTrade pendingArtTrade);
        public Task ChangeOfferStatus(ArtTradeOffer artTradeOffer, TradeStatus tradeStatus);
        public Task EndTrade(ArtTradeOffer offer1, ArtTradeOffer offer2, PendingArtTrade pendingArtTrade);
        public ArtData GetArtDataByFileName(string fileName);
        public Task CreateArtTrade(PendingArtTrade pendingArtTrade);

    }
}
