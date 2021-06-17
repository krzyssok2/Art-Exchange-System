using DATA.Entities;
using LOGIC.Interfaces;
using LOGIC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DATA.Enums;

namespace LOGIC.Services
{
    public class TradeService : ITradeService
    {
        public TradeService()
        {
            
        }

        //public PendingArtTrade GetNewTrade(UserData user, UserData user2, TradeCreationModel tradeCreationModel)
        //{
        //    var userList = new List<UserData> { user, user2 };

        //    var artTreadeOffer1 = new ArtTradeOffer
        //    {
        //        TradeStatus = TradeTransactionStatusEnum.TradeStatus.OfferChange,
        //        User = user,
        //        OferredArtDatas = new List<ArtData>()
        //    };

        //    var artTreadeOffer2 = new ArtTradeOffer
        //    {
        //        TradeStatus = TradeTransactionStatusEnum.TradeStatus.NotifyAboutChange,
        //        User = user2,
        //        OferredArtDatas= new List<ArtData>()
        //    };
        //    foreach (var item in tradeCreationModel.OfferedArt)
        //    {
        //        //var art = _context.ArtData.Include(i => i.CurrentOwner).FirstOrDefault(i => i.FileName == item.ArtFile);
        //        if (art == null) return null;
        //        if (!(art.CurrentOwner.DisplayName == user.DisplayName)) return null;
        //        artTreadeOffer1.OferredArtDatas.Add(art);
        //    }

        //    foreach (var item in tradeCreationModel.WantedArt)
        //    {
        //        //var art = _context.ArtData.Include(i => i.CurrentOwner).First(i => i.FileName == item.ArtFile);
        //        if (art == null) return null;
        //        if (!(art.CurrentOwner.DisplayName == user2.DisplayName)) return null;
        //        artTreadeOffer2.OferredArtDatas.Add(art);
        //    }

        //    var listOffers = new List<ArtTradeOffer> { artTreadeOffer1, artTreadeOffer2 };

        //    var trade = new PendingArtTrade
        //    {
        //        CreationDate = DateTime.Now,
        //        TradingUsers = userList,
        //        UserOffers = listOffers
        //    };

        //    return trade;
        //}
    }
}
