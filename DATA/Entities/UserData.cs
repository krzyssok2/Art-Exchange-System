using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DATA.Entities
{
    public class UserData
    {
        public long Id { get; set; }
        public IdentityUser IdentityUser { get; set; }
        public string DisplayName { get; set; }
        public ICollection<ArtData> OwnedArt { get; set; }
        public ICollection<PendingArtTrade> OngoingTrades { get; set; }
        public ICollection<ArtData> LikedArt { get; set; }
        public ICollection<ArtData> CreatedArt { get; set; }

    }
}
