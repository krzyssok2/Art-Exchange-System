using Art_Exchange_Token_System.Models;
using Art_Exchange_Token_System.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Art_Exchange_Token_System.Enums.TradeTransactionStatusEnum;

namespace Art_Exchange_Token_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TradeController : Controller
    {
        private readonly ArtExchangeContext _context;
        private readonly TradeService _tradeService;
        public TradeController(ArtExchangeContext artExchangeContext, TradeService tradeService)
        {
            _context = artExchangeContext;
            _tradeService = tradeService;
        }

        [HttpGet]
        public async Task<ActionResult<AllOnGoingTradesModel>> GetAllUserTrades()
        {
            var email = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;

            var user = _context.UserData
                .Include(i => i.OngoingTrades).ThenInclude(i => i.TradingUsers)
                .Include(i => i.OngoingTrades).ThenInclude(i => i.UserOffers).ThenInclude(i => i.OferredArtDatas).ThenInclude(i => i.ArtTradeOffers)
                .First(i => i.IdentityUser.Email == email);            

            var trades = user.OngoingTrades;

            if (trades == null) return BadRequest("Not found");

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


            return Ok(requestAnswer);
        }

        [HttpPost]
        public async Task<ActionResult<OwnedArtDataModel>> CreateTrade(TradeCreationModel tradeCreationModel)
        {
            var email = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;

            var user = _context.UserData.First(i => i.IdentityUser.Email == email);

            var user2 = _context.UserData
                .Include(i => i.IdentityUser)
                .First(i => i.DisplayName == tradeCreationModel.SecondTraderUserName);

            var trade = _tradeService.GetNewTrade(user, user2, tradeCreationModel);

            if (trade == null) return BadRequest("Failed to create trade");

            _context.Add(trade);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTradeInfoModel>> GetTrade(long id)
        {
            var email = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;

            var user = _context.UserData.First(i => i.IdentityUser.Email == email);

            var trade = _context.PendingArtTrades
                .Include(i=>i.UserOffers)
                .ThenInclude(i=>i.OferredArtDatas)
                .ThenInclude(i=>i.Catgegory)
                .Include(i=>i.UserOffers).ThenInclude(i=>i.User)
                .FirstOrDefault(i => i.Id == id);

            if (trade == null) return BadRequest("NoTradeFound");

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
            
            return Ok(answer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GetTradeInfoModel>> DeleteTrade(long id)
        {
            var email = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;

            var user = _context.UserData.First(i => i.IdentityUser.Email == email);

            var item = _context.PendingArtTrades
                .Include(i=>i.TradingUsers)
                .Include(i=>i.UserOffers)
                .FirstOrDefault(i => i.Id == id);

            if (item == null) return BadRequest("NotFound");


            bool found=false;
            foreach(var userL in item.TradingUsers)
            {
                if (userL.DisplayName == user.DisplayName)
                {
                    found = true;
                    break;
                }
            }

            if (!found) return BadRequest("No permission to remove");
            
            _context.PendingArtTrades.Remove(item);
            _context.SaveChanges();

            return Ok("Deleted");
        }

        [HttpPatch("{id}/StatusChange")]
        public async Task<ActionResult<GetTradeInfoModel>> ChangeTradeStatus(long id, TradeStatus tradeStatus)
        {
            try
            {
                var email = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;

                var user = _context.UserData.First(i => i.IdentityUser.Email == email);

                var trade = _context.PendingArtTrades
                    .Include(i => i.UserOffers).ThenInclude(i => i.OferredArtDatas)
                    .Include(i => i.UserOffers).ThenInclude(i => i.User)
                    .FirstOrDefault(i => i.Id == id);

                var offer = trade.UserOffers.First(i => i.User.DisplayName == user.DisplayName);

                if (offer == null) return BadRequest("NotAllowed");

                if (offer.TradeStatus == tradeStatus) return Ok();
                else offer.TradeStatus = tradeStatus;

                var offer2 = trade.UserOffers.First(i => i.User.DisplayName != user.DisplayName);

                if (offer2.TradeStatus != TradeStatus.OfferAccepted) offer2.TradeStatus = TradeStatus.NotifyAboutChange;

                if (!(offer.TradeStatus == TradeStatus.OfferAccepted && offer2.TradeStatus == TradeStatus.OfferAccepted)) 
                {
                    _context.SaveChanges();
                    return Ok();
                }

                var userData1 = offer.User;

                var userData2 = offer2.User;

                foreach(var item in offer.OferredArtDatas)
                {
                    userData1.OwnedArt.Remove(item);
                }

                foreach (var item in offer2.OferredArtDatas)
                {
                    userData2.OwnedArt.Remove(item);
                }

                foreach (var item in offer2.OferredArtDatas)
                {
                    userData1.OwnedArt.Add(item);
                }

                foreach (var item in offer.OferredArtDatas)
                {
                    userData2.OwnedArt.Add(item);
                }

                _context.PendingArtTrades.Remove(trade);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            _context.SaveChanges();

            return Ok("Trade successfully over");

        }
    }
}