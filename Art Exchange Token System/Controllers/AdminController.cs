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
        public async Task<ActionResult> GrantRole(GrantRoleModel request)
        {
            var result = await _adminService.GrantRole(request.Mail, request.Role);

            if (!result.Success) return BadRequest(result.Errors);

            return Ok();
        }

        [HttpGet("UserRoles/{mail}")]
        [ProducesResponseType(typeof(UserRolesModel), 200)]
        public async Task<ActionResult> GetUserRoles(string email)
        {
            var result = await _adminService.GetUserRoles(email);

            if (!result.Success) return BadRequest(result.Errors);

            return Ok(result.ResponseData);
        }

        [HttpPatch("RevokeRole")]
        public async Task<ActionResult> RevokeRole(RevokeRoleModel request)
        {
            var result = await _adminService.RevokeUserRole(request.Mail, request.Role);

            if (!result.Success) return BadRequest(result.Errors);

            return Ok();
        }
    }
}
