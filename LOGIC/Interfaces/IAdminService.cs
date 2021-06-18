using LOGIC.Models;
using LOGIC.Models.ErrorHandlingModels;
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
        public Task<ServiceResponseModel> GrantRole(string email, string role);
        public Task<ServiceResponseModel<UserRolesModel>> GetUserRoles(string email);
        public Task<ServiceResponseModel> RevokeUserRole(string email, string role);
    }
}
