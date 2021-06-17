using LOGIC.Models;
using LOGIC.Models.TransactionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthenticationResult> LogIn(string userName, string password);
        public Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
        public Task<CreateUserTransactionModel> RegisterAsync(string userName, string gmail, string password);
    }
}
