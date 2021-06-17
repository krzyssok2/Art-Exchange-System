using DATA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATA.EntityConfiguration
{
    public class UserDataConfiguration : IEntityTypeConfiguration<UserData>
    {
        public void Configure(EntityTypeBuilder<UserData> builder)
        {
            builder.HasMany(i => i.OngoingTrades)
                .WithMany(i => i.TradingUsers);
            builder.HasMany(i => i.OwnedArt)
                .WithOne(i => i.CurrentOwner);
            builder.HasMany(i => i.CreatedArt)
                .WithOne(i => i.OriginalCreator);

            builder.HasOne(i => i.IdentityUser);
        }
    }
}
