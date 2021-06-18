using DATA.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Interfaces
{
    public interface IArtFunctions
    {
        public ArtCategory GetArtCategory(string categoryName);
        public ArtData AddArtData(UserData userData, ArtData artData);
        public ArtData GetArtDataByName(string fileName);
        public Task<bool> IsUserPermited(string fileName, IdentityUser user, ArtData image);
        public Task RemoveArtData(ArtData image);
        public List<ArtData> GetOwnedArtByUserName(string usernName);
        public List<ArtData> GetCreatedArtByUserName(string usernName);

    }
}
