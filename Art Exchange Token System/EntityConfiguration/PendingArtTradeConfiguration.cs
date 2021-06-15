using Art_Exchange_Token_System.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Art_Exchange_Token_System.EntityConfiguration
{
    public class PendingArtTradeConfiguration : IEntityTypeConfiguration<PendingArtTrade>
    {
        public void Configure(EntityTypeBuilder<PendingArtTrade> builder)
        {
            builder.HasMany(i => i.UserOffers)
                .WithOne(i => i.PendingArtTrade)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
