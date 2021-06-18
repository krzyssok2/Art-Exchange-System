using LOGIC.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC.Interfaces
{
    public interface IAdminService
    {
        public Task GrantRole(string email, string role);
        public Task<UserRolesModel> GetUserRoles(string email);
        public Task RevokeUserRole(string email, string role);
    }
}
