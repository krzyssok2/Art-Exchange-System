using Art_Exchange_Token_System.Models;
using Art_Exchange_Token_System.Models.RequestModels;
using Art_Exchange_Token_System.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AdminController(UserManager<IdentityUser> identityService)
        {
            _userManager = identityService;
        }

        [HttpPost("GrantRole")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        public async Task<ActionResult> GrantRole(GrantRoleModel request)
        {
            var user = _userManager.FindByEmailAsync(request.Mail);

            var roleResponse = await _userManager.AddToRoleAsync(user.Result, request.Role);

            if (!roleResponse.Succeeded)
            {
                return BadRequest(new 
                {
                    Errors = roleResponse.Errors
                });
            }
            return Ok(user.Result);
        }

        [HttpGet("UserRoles/{mail}")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        public async Task<ActionResult> GetUserRoles(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);

            if (user == null) return NotFound();


            var roleResponse = (await _userManager.GetRolesAsync(user)).ToList();

            return Ok(new UserRolesModel
            {
                UserMail = mail,
                Roles = roleResponse
            });        
        }

        [HttpPatch("RevokeRole")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        public async Task<ActionResult> RevokeRole(RevokeRoleModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Mail);

            var revokeRequest = await _userManager.RemoveFromRoleAsync(user, request.Role);

            if (!revokeRequest.Succeeded)
            {                
                return BadRequest(new
                {
                    Errors = revokeRequest.Errors
                });
            }

            var roleResponse = (await _userManager.GetRolesAsync(user)).ToList();
            return Ok(new UserRolesModel
            {
                UserMail = request.Mail,
                Roles = roleResponse
            });
        }
    }
}
