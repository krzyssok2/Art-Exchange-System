using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Art_Exchange_Token_System.Entities
{
    public class ArtData
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ArtCategory Catgegory { get; set; }
        public ICollection<UserData> LikedUsers { get; set; }
        public UserData OriginalCreator { get; set; }
        public ICollection<UserData> PreviousOwners { get; set; }
        public UserData CurrentOwner { get; set; }
        public string FileName { get; set; }
        public ICollection<ArtTradeOffer> ArtTradeOffers { get; set; }
    }
}
