using DATA.Functions;
using DATA.Interfaces;
using LOGIC.Interfaces;
using LOGIC.Models;
using LOGIC.Models.TransactionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC.Services
{
    public class AdminService: IAdminService
    {
        public IAccountFunctions _accountFunctions;
        public IAdminFunctions _adminFunctions;
        public AdminService(IAccountFunctions accountFunctions, IAdminFunctions adminFunctions)
        {
            _accountFunctions = accountFunctions;
            _adminFunctions = adminFunctions;
        }

        public async Task GrantRole(string email,string role)
        {
            var user = await _accountFunctions.GetUserByEmail(email);

            await _adminFunctions.GrantUserRole(user, role);
        }

        public async Task<UserRolesModel> GetUserRoles(string email)
        {
            var user = await _accountFunctions.GetUserByEmail(email);

            //if (user == null) return NotFound();


            var roleResponse = await _adminFunctions.GetUserRoles(user);

            return new UserRolesModel
            {
                UserMail = email,
                Roles = roleResponse.ToList()
            };
        }

        public async Task RevokeUserRole(string email,string role)
        {
            var user = await _accountFunctions.GetUserByEmail(email);

            await _adminFunctions.RevokeRole(user, role);
        }
    }
}
