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
using DATA.Functions;
using LOGIC.Models.TransactionModels;
using static DATA.Enums.TradeTransactionStatusEnum;
using DATA.Interfaces;
using LOGIC.Models.ErrorHandlingModels;

namespace LOGIC.Services
{
    public class TradeService : ITradeService
    {
        public IAccountFunctions _accountFunctions;
        public ITradeFunctions _tradeFunctions;
        public TradeService(IAccountFunctions accountFuntions, ITradeFunctions tradeFunctions)
        {
            _accountFunctions = accountFuntions;
            _tradeFunctions = tradeFunctions;
        }

        public async Task<ServiceResponseModel<AllOnGoingTradesModel>> GetAllUserTradesByEmail(string email)
        {
            var userData = _tradeFunctions.GetUserDataDetailedArtOffers(email);

            var trades = userData.OngoingTrades;

            if (trades == null) return new ServiceResponseModel<AllOnGoingTradesModel>
            {
                Success=true,
                ResponseData = new AllOnGoingTradesModel()
                {
                    OnGoingTrades= new List<OnGoingTradeModel>()
                }
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


            return new ServiceResponseModel<AllOnGoingTradesModel>
            {
                Success=true,
                ResponseData=requestAnswer
            };
        }

        public async Task<ServiceResponseModel> DeleteTradeByIdAsync(string email,long id)
        {
            var user = _accountFunctions.GetUserByEmail(email).Result;

            var userData = _accountFunctions.GetUserDataByUser(user).Result;

            var pendingArtTrade =  _tradeFunctions.GetPendingArtTradeById(id);

            if (pendingArtTrade == null) return new ServiceResponseModel
            {
                Success = false,
                Errors= new List<Error>
                {
                    new Error
                    {
                        Code=400,
                        Message=ErrorEnum.NotFound
                    }
                }
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

            if (!found) return new  ServiceResponseModel
            {
                Success = false,
                Errors = new List<Error>
                {
                    new Error
                    {
                        Code=400,
                        Message=ErrorEnum.NotPermitted
                    }
                }
            };

            await _tradeFunctions.DeleteTrade(pendingArtTrade);

            return new ServiceResponseModel
            {
                Success = true,
                Errors = new List<Error>
                {
                    new Error
                    {
                        Code=400,
                        Message=ErrorEnum.NotPermitted
                    }
                }
            };
        }

        public async Task<ServiceResponseModel<GetTradeInfoModel>> GetTradeInfoById(string email, long id)
        {
            var user = await _accountFunctions.GetUserByEmail(email);

            var trade = _tradeFunctions.GetPendingArtTradeById(id);

            if (trade == null) return new ServiceResponseModel<GetTradeInfoModel>
            {
                Success=false,
                Errors = new List<Error>
                {
                    new Error
                    {
                        Code=400,
                        Message= ErrorEnum.NotFound
                    }
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

            return new ServiceResponseModel<GetTradeInfoModel>
            {
                Success=true,
                ResponseData=answer
            };
        }

        public async Task<ServiceResponseModel> ChangeTradeStatusAsync(string email,long id, TradeStatus tradeStatus)
        {
            var trade = _tradeFunctions.GetPendingArtTradeById(id);

            if (trade == null) return new ServiceResponseModel
            {               
                Success = false,
                Errors= new List<Error>
                {
                    new Error
                    {
                        Code=400,
                        Message=ErrorEnum.NotFound
                    }
                }
            };

            var user = _accountFunctions.GetUserByEmail(email).Result;

            var userData = _accountFunctions.GetUserDataByUser(user).Result;

            var offer = trade.UserOffers.First(i => i.User.DisplayName == userData.DisplayName);

            if (offer == null) return new ServiceResponseModel
            {
                Success = false,
                Errors = new List<Error>
                {
                    new Error
                    {
                        Code=400,
                        Message=ErrorEnum.NotFound
                    }
                }
            };

            if (offer.TradeStatus == tradeStatus) return new ServiceResponseModel
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
                return new ServiceResponseModel
                {
                    Success = true
                };
            }

            await _tradeFunctions.EndTrade(offer, offer2, trade);

            return new ServiceResponseModel
            {
                Success = true
            };
        }

        public async Task<ServiceResponseModel<GetTradeInfoModel>> PostNewTradeAsync(string email,TradeCreationModel tradeCreationModel)
        {
            var userData = _accountFunctions.GetUserDataByEmail(email);

            var user2 = _accountFunctions.GetUserDataByUserName(tradeCreationModel.SecondTraderUserName);

            var trade = GetNewTrade(userData, user2, tradeCreationModel);

            if (trade == null)
            {
                return new ServiceResponseModel<GetTradeInfoModel>
                {
                    Success = false,
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Code=400,
                            Message= ErrorEnum.FailedToCreate
                        }
                    }
                };
            }

            await _tradeFunctions.CreateArtTrade(trade);

            return new ServiceResponseModel<GetTradeInfoModel>
            {
                Success = true,
                ResponseData = new GetTradeInfoModel
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
