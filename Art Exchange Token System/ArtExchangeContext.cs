using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Art_Exchange_Token_System.Entities;
using Art_Exchange_Token_System.EntityConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Art_Exchange_Token_System
{
    public class ArtExchangeContext : IdentityDbContext<IdentityUser>
    {
        public ArtExchangeContext(DbContextOptions options) : base(options) { }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserData> UserData { get; set; }
        public DbSet<ArtData> ArtData { get; set; }
        public DbSet<PendingArtTrade> PendingArtTrades { get; set; }
        public DbSet<ArtCategory> ArtCategories { get; set; }
        public DbSet<ArtTradeOffer> ArtTradeOffers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.ApplyConfiguration(new ArtDataConfiguration());
            builder.ApplyConfiguration(new UserDataConfiguration());
            builder.ApplyConfiguration(new PendingArtTradeConfiguration());
        }
    }
}
