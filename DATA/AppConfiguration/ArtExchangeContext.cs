using DATA.Entities;
using DATA.EntityConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DATA.AppConfiguration
{
    public class ArtExchangeContext : IdentityDbContext<IdentityUser>
    {

        public static OptionsBuild Options = new();
        public class OptionsBuild
        {
            public OptionsBuild()
            {
                Settings = new AppConfiguration();

                OptionsBuilder = new DbContextOptionsBuilder<ArtExchangeContext>();

                OptionsBuilder.UseSqlServer(Settings.SqlConnectionString);

                DatabaseOptions = OptionsBuilder.Options;
            }
            public DbContextOptionsBuilder<ArtExchangeContext> OptionsBuilder { get; set; }

            public DbContextOptions<ArtExchangeContext> DatabaseOptions { get; set; }

            private AppConfiguration Settings { get; set; }
        }


        public ArtExchangeContext(DbContextOptions<ArtExchangeContext> options) : base(options) { }

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
