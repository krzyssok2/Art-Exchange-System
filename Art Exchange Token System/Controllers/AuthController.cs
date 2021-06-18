using LOGIC.Interfaces;
using LOGIC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Art_Exchange_Token_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        /// <summary>
        /// Log in user
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Successfully logged in</response>   
        /// <response code="400">Wasn't able to log in</response>   
        [HttpPost("Login")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        public async Task<ActionResult> LoginAsync(AuthAccount request)
        {
            var result = await _authService.LogIn(request.UserName, request.Password);

            if (!result.Success) return BadRequest(result.Errors);

            return Ok(result.ResponseData);
        }
        /// <summary>
        /// Refresh user token
        /// </summary>
        /// <returns></returns>
        [HttpPost("Refresh")]
        public async Task<ActionResult> RefreshAsync(RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!result.Success) return BadRequest(result.Errors);

            return Ok(result.ResponseData);
        }
        /// <summary>
        /// Register user
        /// </summary>
        /// <returns></returns>
        [HttpPost("Registration")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(IdentityError), 400)]
        public async Task<ActionResult> RegisterAsync(RegisterAccountModel request)
        {
            var result = await _authService.RegisterAsync(request.UserName, request.Email, request.Password);
            if(!result.Success)
            {
                return BadRequest(new
                {
                    result.Errors
                });
            }    
            return Ok();
        }
    }
}
