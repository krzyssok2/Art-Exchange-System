using DATA.AppConfiguration;
using DATA.Entities;
using DATA.Functions;
using DATA.Interfaces;
using LOGIC.Interfaces;
using LOGIC.Models;
using LOGIC.Models.TransactionModels;
using LOGIC.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC.Services
{
    public class AuthService: IAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IAccountFunctions _accountFunctions;
        public AuthService(JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, ArtExchangeContext artExchangeContext, UserManager<IdentityUser> userManager,
            IAccountFunctions accountFunctions)
        {
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _accountFunctions = accountFunctions;
        }
        public async Task<AuthenticationResult> LogIn(string userName, string password)
        {
            var user = await _accountFunctions.GetUserByUserNameAsync(userName);

            if (user == null)
            {
                return new AuthenticationResult(Errors.UserDoesntExist);
            }

            var userHasValidPassword = await _accountFunctions.IsPasswordCorrect(user, password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResult(Errors.PassNickWrong);
            }

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return new AuthenticationResult(Errors.InvalidToken);
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken =await _accountFunctions.GetToken(refreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthenticationResult(Errors.TokenDoesntExist);
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticationResult(Errors.RefreshTokenExpired);
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationResult(Errors.RefreshTokenInvalid);
            }

            if (storedRefreshToken.Used)
            {
                return new AuthenticationResult(Errors.RefreshTokenUsed);
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult(Errors.RefreshTokenDoesntMatchJwT);
            }

            storedRefreshToken.Used = true;

            _accountFunctions.ModifyRefreshToken(storedRefreshToken);

            var user = await _accountFunctions.GetIdentityUserByTokenClaim(validatedToken);
            return await GenerateAuthenticationResultForUserAsync(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(IdentityUser user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, System.Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };
            _accountFunctions.AddRefreshToken(refreshToken);

            return new AuthenticationResult()
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<CreateUserTransactionModel> RegisterAsync(string userName, string gmail, string password)
        {
            var result = await _accountFunctions.CreateUser(userName, gmail, password);            

            if (!result.Succeeded)
            {
                return new CreateUserTransactionModel
                {
                    Success = false,
                    Errors = result.Errors.ToList(),
                };
            }

            var identity = await _accountFunctions.GetUserByEmail(gmail);            

            await _accountFunctions.AddRoleToUser(identity, "User");

            await _accountFunctions.CreateUserDataToUser(userName, gmail);

            return new CreateUserTransactionModel
            {
                Success = true
            };
        }
    }
}