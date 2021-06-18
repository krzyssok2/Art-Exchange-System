using LOGIC.Models;
using LOGIC.Models.ErrorHandlingModels;
using LOGIC.Models.TransactionModels;
using System.Threading.Tasks;

namespace LOGIC.Interfaces
{
    public interface IAuthService
    {
        public Task<ServiceResponseModel<AuthSuccessResponse>> LogIn(string userName, string password);
        public Task<ServiceResponseModel<AuthSuccessResponse>> RefreshTokenAsync(string token, string refreshToken);
        public Task<CreateUserTransactionModel> RegisterAsync(string userName, string gmail, string password);
    }
}
