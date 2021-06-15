using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Art_Exchange_Token_System.Entities
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
