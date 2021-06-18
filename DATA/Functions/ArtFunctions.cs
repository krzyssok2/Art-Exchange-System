using DATA.AppConfiguration;
using DATA.Entities;
using DATA.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Functions
{
    public class ArtFunctions : IArtFunctions
    {
        private readonly ArtExchangeContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ArtFunctions(ArtExchangeContext artExchangeContext, UserManager<IdentityUser> userManager)
        {
            _context = artExchangeContext;
            _userManager = userManager;
        }
        public ArtCategory GetArtCategory(string categoryName)
        {
            return _context.ArtCategories.FirstOrDefault(i => i.CategoryName == categoryName);
        }

        public ArtData AddArtData(UserData userData,ArtData artData)
        {
            _context.ArtData.Add(artData);
            userData.OwnedArt.Add(artData);

            _context.SaveChangesAsync();

            return artData;
        }

        public ArtData GetArtDataByName(string fileName)
        {
            return _context.ArtData
                .Include(i => i.CurrentOwner)
                .FirstOrDefault(i => i.FileName == fileName);
        }

        public async Task<bool> IsUserPermited(string fileName, IdentityUser user, ArtData image)
        {
            return await _userManager.IsInRoleAsync(user, "admin")
                || image.CurrentOwner.DisplayName == user.UserName;
        }

        public async Task RemoveArtData(ArtData image)
        {
            _context.ArtData.Remove(image);
            await _context.SaveChangesAsync();
        }

        public List<ArtData> GetOwnedArtByUserName(string usernName)
        {
            return _context.ArtData
                .Include(i => i.CurrentOwner)
                .Include(i => i.Catgegory)
                .Where(i => i.CurrentOwner.DisplayName == usernName).ToList();
        }

        public List<ArtData> GetCreatedArtByUserName(string usernName)
        {
            return _context.ArtData
               .Include(i => i.OriginalCreator)
               .Include(i => i.Catgegory)
               .Where(i => i.OriginalCreator.DisplayName == usernName).ToList();
        }
    }
}
