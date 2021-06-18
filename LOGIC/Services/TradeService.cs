﻿using DATA.Entities;
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
using DATA.Functions;
using LOGIC.Models.TransactionModels;
using static DATA.Enums.TradeTransactionStatusEnum;

namespace LOGIC.Services
{
    public class TradeService : ITradeService
    {
        public AccountFunctions _accountFunctions;
        public TradeFunctions _tradeFunctions;
        public TradeService(AccountFunctions accountFuntions, TradeFunctions tradeFunctions)
        {
            _accountFunctions = accountFuntions;
            _tradeFunctions = tradeFunctions;
        }

        public AllOnGoingTradesModel GetAllUserTradesByEmail(string email)
        {
            var userData = _tradeFunctions.GetUserDataDetailedArtOffers(email);

            var trades = userData.OngoingTrades;

            if (trades == null) return new AllOnGoingTradesModel
            {
                OnGoingTrades = new List<OnGoingTradeModel>()
            };

            var requestAnswer = new AllOnGoingTradesModel
            {
                OnGoingTrades = trades.Select(i => new OnGoingTradeModel
                {
                    TradeId = i.Id,
                    TradingUsers = i.UserOffers.Select(j => new UserInformationModel
                    {
                        UserName = j.User.DisplayName,
                        TradeStatus = j.TradeStatus,
                        OfferedArt = j.OferredArtDatas.Select(k => new TradeInformation
                        {
                            Name = k.Name,
                            Description = k.Description,
                            FileName = k.FileName
                        }).ToList()
                    }).ToList()
                }).ToList()
            };


            return requestAnswer;
        }

        public async Task<ErrorHandlingModel> DeleteTradeByIdAsync(string email,long id)
        {
            var user = _accountFunctions.GetUserByEmail(email).Result;

            var userData = _accountFunctions.GetUserDataByUser(user).Result;

            var pendingArtTrade =  _tradeFunctions.GetPendingArtTradeById(id);

            if (pendingArtTrade == null) return new ErrorHandlingModel
            {
                Success = false,
                Error = "Not found"
            };


            bool found = false;
            foreach (var userL in pendingArtTrade.TradingUsers)
            {
                if (userL.DisplayName == userData.DisplayName)
                {
                    found = true;
                    break;
                }
            }

            if (!found) return new ErrorHandlingModel
            {
                Success = false,
                Error = "Not permission"
            };

            await _tradeFunctions.DeleteTrade(pendingArtTrade);

            return new ErrorHandlingModel
            {
                Success = true
            };
        }

        public GetTradeTransactionModel GetTradeInfoById(string email, long id)
        {
            var user = _accountFunctions.GetUserByEmail(email);

            var trade = _tradeFunctions.GetPendingArtTradeById(id);

            if (trade == null) return new GetTradeTransactionModel
            {
                ErrorHandling = new ErrorHandlingModel
                {
                    Success = false,
                    Error = "NotFound"
                }
            };

            var answer = new GetTradeInfoModel
            {
                Id = trade.Id,
                UserTrades = trade.UserOffers.Select(i => new UserTradeOfferModel
                {
                    Username = i.User.DisplayName,
                    tradeStatus = i.TradeStatus,
                    OfferedArt = i.OferredArtDatas.Select(j => new ArtInfoModel
                    {
                        Name = j.Name,
                        Description = j.Description,
                        Category = j.Catgegory.CategoryName,
                        FileName = j.FileName
                    }).ToList()
                }).ToList()
            };

            return new GetTradeTransactionModel
            {
                ErrorHandling = new ErrorHandlingModel
                {
                    Success = true
                },
                TradeInfo= answer
            };
        }

        public async Task<ErrorHandlingModel> ChangeTradeStatusAsync(string email,long id, TradeStatus tradeStatus)
        {
            var trade = _tradeFunctions.GetPendingArtTradeById(id);

            if (trade == null) return new ErrorHandlingModel
            {
                Error = "not found",
                Success = false
            };

            var user = _accountFunctions.GetUserByEmail(email).Result;

            var userData = _accountFunctions.GetUserDataByUser(user).Result;

            var offer = trade.UserOffers.First(i => i.User.DisplayName == userData.DisplayName);

            if (offer == null) return new ErrorHandlingModel
            {
                Success = false,
                Error = "Not found"
            };

            if (offer.TradeStatus == tradeStatus) return new ErrorHandlingModel
            {
                Success = true,
            };
            else offer.TradeStatus = tradeStatus;

            var offer2 = trade.UserOffers.First(i => i.User.DisplayName != userData.DisplayName);

            if (offer2.TradeStatus != TradeStatus.OfferAccepted)
            {
                await _tradeFunctions.ChangeOfferStatus(offer2, tradeStatus);
            }

            if (!(offer.TradeStatus == TradeStatus.OfferAccepted && offer2.TradeStatus == TradeStatus.OfferAccepted))
            {
                return new ErrorHandlingModel
                {
                    Success = true
                };
            }

            await _tradeFunctions.EndTrade(offer, offer2, trade);

            return new ErrorHandlingModel
            {
                Success = true
            };
        }

        public async Task<PostArtTransactionResult> PostNewTradeAsync(string email,TradeCreationModel tradeCreationModel)
        {
            var userData = _accountFunctions.GetUserDataByEmail(email);

            var user2 = _accountFunctions.GetUserDataByUserName(tradeCreationModel.SecondTraderUserName);

            var trade = GetNewTrade(userData, user2, tradeCreationModel);

            if (trade == null)
            {
                return new PostArtTransactionResult
                {
                    Success = false,
                    Error = "failed to create"
                };
            }

            await _tradeFunctions.CreateArtTrade(trade);

            return new PostArtTransactionResult
            {
                Success = true,
                ArtData = new GetTradeInfoModel
                {
                    Id = trade.Id,
                    UserTrades = trade.UserOffers.Select(i => new UserTradeOfferModel
                    {
                        Username = i.User.DisplayName,
                        tradeStatus = i.TradeStatus,
                        OfferedArt = i.OferredArtDatas.Select(j => new ArtInfoModel
                        {
                            Name = j.Name,
                            Description = j.Description,
                            Category = j.Catgegory.CategoryName,
                            FileName = j.FileName
                        }).ToList()
                    }).ToList()
                }
            };
        }

        private PendingArtTrade GetNewTrade(UserData user, UserData user2, TradeCreationModel tradeCreationModel)
        {
            var userList = new List<UserData> { user, user2 };

            var artTreadeOffer1 = new ArtTradeOffer
            {
                TradeStatus = TradeStatus.OfferChange,
                User = user,
                OferredArtDatas = new List<ArtData>()
            };

            var artTreadeOffer2 = new ArtTradeOffer
            {
                TradeStatus = TradeStatus.NotifyAboutChange,
                User = user2,
                OferredArtDatas = new List<ArtData>()
            };
            foreach (var item in tradeCreationModel.OfferedArt)
            {
                var art = _tradeFunctions.GetArtDataByFileName(item.ArtFile);
                if (art == null) return null;
                if (!(art.CurrentOwner.DisplayName == user.DisplayName)) return null;
                artTreadeOffer1.OferredArtDatas.Add(art);
            }

            foreach (var item in tradeCreationModel.WantedArt)
            {
                var art = _tradeFunctions.GetArtDataByFileName(item.ArtFile);
                if (art == null) return null;
                if (!(art.CurrentOwner.DisplayName == user2.DisplayName)) return null;
                artTreadeOffer2.OferredArtDatas.Add(art);
            }

            var listOffers = new List<ArtTradeOffer> { artTreadeOffer1, artTreadeOffer2 };

            var trade = new PendingArtTrade
            {
                CreationDate = DateTime.Now,
                TradingUsers = userList,
                UserOffers = listOffers
            };

            return trade;
        }
    }
}
