using DATA.AppConfiguration;
using DATA.Entities;
using DATA.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static DATA.Enums.TradeTransactionStatusEnum;

namespace DATA.Functions
{
    public class TradeFunctions :ITradeFunctions
    {
        private readonly ArtExchangeContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public TradeFunctions(ArtExchangeContext artExchangeContext, UserManager<IdentityUser> userManager)
        {
            _context = artExchangeContext;
            _userManager = userManager;
        }

        public UserData GetUserDataDetailedArtOffers(string email)
        {
            return _context.UserData
                .Include(i => i.OngoingTrades).ThenInclude(i => i.TradingUsers)
                .Include(i => i.OngoingTrades).ThenInclude(i => i.UserOffers).ThenInclude(i => i.OferredArtDatas).ThenInclude(i => i.ArtTradeOffers)
                .First(i => i.IdentityUser.Email == email);
        }

        public PendingArtTrade GetPendingArtTradeById(long id)
        {
            return _context.PendingArtTrades
                .Include(i => i.UserOffers)
                .ThenInclude(i => i.OferredArtDatas)
                .ThenInclude(i => i.Catgegory)
                .Include(i=>i.TradingUsers)
                .Include(i => i.UserOffers).ThenInclude(i => i.User)
                .FirstOrDefault(i => i.Id == id);
        }

        public async Task DeleteTrade(PendingArtTrade pendingArtTrade)
        {
            _context.PendingArtTrades.Remove(pendingArtTrade);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeOfferStatus(ArtTradeOffer artTradeOffer, TradeStatus tradeStatus)
        {
            artTradeOffer.TradeStatus = TradeStatus.NotifyAboutChange;
            await _context.SaveChangesAsync();            
        }

        public async Task EndTrade(ArtTradeOffer offer1, ArtTradeOffer offer2, PendingArtTrade pendingArtTrade)
        {
            var userData1 = offer1.User;

            var userData2 = offer2.User;

            foreach (var item in offer1.OferredArtDatas)
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

            foreach (var item in offer1.OferredArtDatas)
            {
                userData2.OwnedArt.Add(item);
            }

            _context.PendingArtTrades.Remove(pendingArtTrade);
            await _context.SaveChangesAsync();
        }

        public ArtData GetArtDataByFileName(string fileName)
        {
            return _context.ArtData
                .Include(i => i.CurrentOwner)
                .Include(i=>i.Catgegory)
                .FirstOrDefault(i => i.FileName == fileName);
        }

        public async Task CreateArtTrade(PendingArtTrade pendingArtTrade)
        {
            _context.Add(pendingArtTrade);
            await _context.SaveChangesAsync();
        }
    }
}
