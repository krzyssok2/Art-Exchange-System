using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Interfaces
{
    public interface IAdminFunctions
    {
        public Task GrantUserRole(IdentityUser identityUser, string role);
        public Task<IList<string>> GetUserRoles(IdentityUser identityUser);
        public Task RevokeRole(IdentityUser identityUser, string role);

    }
}
