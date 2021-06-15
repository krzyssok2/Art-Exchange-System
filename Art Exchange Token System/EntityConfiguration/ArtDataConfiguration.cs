using Art_Exchange_Token_System.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Art_Exchange_Token_System.EntityConfiguration
{
    public class ArtDataConfiguration : IEntityTypeConfiguration<ArtData>
    {
        public void Configure(EntityTypeBuilder<ArtData> builder)
        {
            builder.HasMany(i => i.LikedUsers)
                .WithMany(i => i.LikedArt);
            
            builder.HasOne(i => i.OriginalCreator)
                .WithMany(i => i.CreatedArt);
            builder.HasMany(i => i.PreviousOwners);

            builder.HasMany(i => i.ArtTradeOffers).WithMany(i => i.OferredArtDatas);

        }
    }
}
