using DATA.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DATA.Interfaces
{
    public interface IAccountFunctions
    {
        public Task<IdentityUser> GetUserByUserNameAsync(string userName);
        public Task<bool> IsPasswordCorrect(IdentityUser user, string password);
        public Task RegisterUser(string userName, string gmail, string password);
        public Task<RefreshToken> GetToken(string refreshToken);
        public void ModifyRefreshToken(RefreshToken refreshToken);
        public void AddRefreshToken(RefreshToken refreshToken);
        public Task<IdentityUser> GetIdentityUserByTokenClaim(ClaimsPrincipal validatedToken);
        public Task<IdentityResult> CreateUser(string userName, string email, string password);
        public Task<IdentityUser> GetUserByEmail(string email);
        public Task AddRoleToUser(IdentityUser identity, string role);
        public Task CreateUserDataToUser(string userName, string email);
        public Task<UserData> GetUserDataByUser(IdentityUser identityUser);
        public UserData GetUserDataByUserName(string userName);
        public UserData GetUserDataByEmail(string email);



    }
}
