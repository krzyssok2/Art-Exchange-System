using DATA.AppConfiguration;
using DATA.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DATA.Functions
{
    public class AdminFunctions:IAdminFunctions
    {

        private readonly UserManager<IdentityUser> _userManager;
        public AdminFunctions(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GrantUserRole(IdentityUser identityUser, string role)
        {
            await _userManager.AddToRoleAsync(identityUser, role);
        }
        public async Task<IList<string>> GetUserRoles(IdentityUser identityUser)
        {
            var list= await _userManager.GetRolesAsync(identityUser);
            return list;
        }

        public async Task RevokeRole(IdentityUser identityUser, string role)
        {
            await _userManager.RemoveFromRoleAsync(identityUser, role);
        }

    }
}
