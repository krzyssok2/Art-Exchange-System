using Art_Exchange_Token_System.Models;
using Art_Exchange_Token_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Art_Exchange_Token_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ArtExchangeContext _context;
        private readonly AuthServices _authService;
        public AuthController(UserManager<IdentityUser> identityService, ArtExchangeContext artExchangeContext, AuthServices authservices
            )
        {
            _userManager = identityService;
            _context = artExchangeContext;
            _authService = authservices;
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
            var authResponse = await _authService.LogIn(request.UserName, request.Password);

            if (!authResponse.Success)
            {
                return Unauthorized(new AuthFailedResponse
                {
                    Error = authResponse.Error
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
        /// <summary>
        /// Refresh user token
        /// </summary>
        /// <returns></returns>
        [HttpPost("Refresh")]
        public async Task<ActionResult> RefreshAsync(RefreshTokenRequest request)
        {
            var authResponse = await _authService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Error = authResponse.Error
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
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
            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email,                
            }, request.Password);

            
            if (!result.Succeeded)
            {
                return BadRequest(new
                {                    
                    result.Errors,
                });
            }

            var identity = _context.Users.First(i => i.Email == request.Email);

            _context.SaveChanges();

            await _userManager.AddToRoleAsync(identity, "User");

            _context.UserData.Add(new Entities.UserData
            {
                DisplayName = request.UserName,
                IdentityUser = _context.Users.First(i => i.Email == request.Email),
            });

            _context.SaveChanges();
            return Ok();
        }
    }
}
