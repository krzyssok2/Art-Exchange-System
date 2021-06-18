using Art_Exchange_Token_System.Models;
using Art_Exchange_Token_System.Models.RequestModels;
using LOGIC.Interfaces;
using LOGIC.Models;
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
        private readonly IAdminService _adminService;
        public AdminController(IAdminService authService)
        {
            _adminService = authService;
        }

        [HttpPost("GrantRole")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        public async Task<ActionResult> GrantRole(GrantRoleModel request)
        {
            await _adminService.GrantRole(request.Mail, request.Role);

            return Ok();
        }

        [HttpGet("UserRoles/{mail}")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        public async Task<ActionResult> GetUserRoles(string email)
        {
            var result = await _adminService.GetUserRoles(email);

            return Ok(result);
        }

        [HttpPatch("RevokeRole")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        public async Task<ActionResult> RevokeRole(RevokeRoleModel request)
        {
            await _adminService.RevokeRole(request.Mail, request.Role);
        }
    }
}
