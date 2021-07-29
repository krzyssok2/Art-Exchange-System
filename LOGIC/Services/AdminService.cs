using DATA.Interfaces;
using LOGIC.Interfaces;
using LOGIC.Models;
using LOGIC.Models.ErrorHandlingModels;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ServiceResponseModel> GrantRole(string email,string role)
        {
            var user = await _accountFunctions.GetUserByEmail(email);

            if(user == null)
            {
                return new ServiceResponseModel
                {
                    Success = false,
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Code=400,
                            Message=ErrorEnum.NotFound
                        }
                    }
                };
            }

            await _adminFunctions.GrantUserRole(user, role);

            return new ServiceResponseModel
            {
                Success = true
            };
        }

        public async Task<ServiceResponseModel<UserRolesModel>> GetUserRoles(string email)
        {
            var user = await _accountFunctions.GetUserByEmail(email);

            if (user == null)
            {
                return new ServiceResponseModel<UserRolesModel>
                {
                    Success = false,
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Code=400,
                            Message=ErrorEnum.NotFound
                        }
                    }
                };
            }

            var roleResponse = await _adminFunctions.GetUserRoles(user);

            return new ServiceResponseModel<UserRolesModel>
            {
                Success = true,
                ResponseData = new UserRolesModel
                {
                    UserMail = email,
                    Roles = roleResponse.ToList()
                }
            };
        }

        public async Task<ServiceResponseModel> RevokeUserRole(string email,string role)
        {
            var user = await _accountFunctions.GetUserByEmail(email);

            if (user == null)
            {
                return new ServiceResponseModel
                {
                    Success = false,
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Code=400,
                            Message=ErrorEnum.NotFound
                        }
                    }
                };
            }

            await _adminFunctions.RevokeRole(user, role);

            return new ServiceResponseModel
            {
                Success = true
            };
        }
    }
}
