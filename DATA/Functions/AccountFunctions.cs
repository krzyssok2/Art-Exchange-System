using DATA.AppConfiguration;
using DATA.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using DATA.Interfaces;

namespace DATA.Functions
{
    public class AccountFunctions: IAccountFunctions
    {
        private readonly ArtExchangeContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public AccountFunctions(ArtExchangeContext artExchangeContext, UserManager<IdentityUser> userManager)
        {
            _context = artExchangeContext;
            _userManager = userManager;
        }

        public async Task<IdentityUser> GetUserByUserNameAsync(string userName)
        {
            var item = await _userManager.FindByNameAsync(userName);
            return item;
        }

        public async Task<bool> IsPasswordCorrect(IdentityUser user, string password)
        {
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
            return isPasswordCorrect;
        }

        public async Task RegisterUser(string userName, string gmail, string password)
        {
            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = userName,
                Email = gmail,
            }, password);
        }

        public async Task<RefreshToken> GetToken(string refreshToken)
        {
            var answerToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
            return answerToken;
        }

        public void ModifyRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            _context.SaveChangesAsync();
        }

        public void AddRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChangesAsync();
        }

        public async Task<IdentityUser> GetIdentityUserByTokenClaim(ClaimsPrincipal validatedToken)
        {
            var a = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
            return a;
        }

        public async Task<IdentityResult> CreateUser(string userName, string email, string password)
        {
            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = userName,
                Email = email,
            }, password);

            return result;
        }

        public async Task<IdentityUser> GetUserByEmail(string email)
        {
            return _context.Users.First(i => i.Email == email);
        }

        public async Task AddRoleToUser(IdentityUser identity, string role)
        {
            await _userManager.AddToRoleAsync(identity, role);
            await _context.SaveChangesAsync();
        }

        public async Task CreateUserDataToUser(string userName, string email)
        {
            _context.UserData.Add(new UserData
            {
                DisplayName = userName,
                IdentityUser = _context.Users.First(i => i.Email == email),
            });

            _context.SaveChanges();
        }

        public async Task<UserData> GetUserDataByUser(IdentityUser identityUser)
        {
            var userdata = _context.UserData
                .First(i => i.IdentityUser.Id == identityUser.Id);

            return userdata;
        }

        public UserData GetUserDataByUserName(string userName)
        {
            return _context.UserData
                .Include(i => i.IdentityUser)
                .First(i => i.DisplayName == userName);
        }

        public UserData GetUserDataByEmail(string email)
        {
            return _context.UserData.First(i => i.IdentityUser.Email == email);
        }
    }
}
