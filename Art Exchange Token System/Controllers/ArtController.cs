using Art_Exchange_Token_System.Models;
using Art_Exchange_Token_System.Models.RequestModels;
using LOGIC.Interfaces;
using LOGIC.Models;
using LOGIC.Models.ErrorHandlingModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Art_Exchange_Token_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtController : Controller
    {
        private readonly IArtService _artService;
        public ArtController(IArtService artService)
        {
            _artService = artService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ArtDataCreationModel>> PostArt([FromForm] PostArtModel request)
        {
            var userEmail = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;
            var result = await _artService.AddNewImageAsync(userEmail, request.Name, request.Description, request.Category, request.File);

            if (!result.Success) return BadRequest(result.Errors);

            return Ok(result.ResponseData);
            
        }

        [HttpDelete("{fileName}")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteArt(string fileName)
        {
            var email = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;

            var result = await _artService.DeleteArt(email, fileName);

            if (!result.Success) return BadRequest(result.Errors);

            return Ok(result);

        }

        [HttpGet("{fileName}")]
        public async Task<ActionResult> GetPicture(string fileName)
        {
            var b = _artService.GetFileBytes(fileName);

            if(b==null) return BadRequest(new List<Error>
                {
                    new Error
                    {
                        Code=400,
                        Message=ErrorEnum.NotFound
                    }
                });

            string extension = fileName[(fileName.IndexOf('.') + 1)..];

            return File(b, "image/" + extension);
        }

        [HttpGet("owned/{username}")]
        public async Task<ActionResult<ArtListModel>> GetOwnedArt(string username)
        {
            var result = await _artService.GetOwnedArt(username);

            if (!result.Success) return BadRequest(result.Errors);

            return Ok(result.ResponseData);
        }

        [HttpGet("created/{username}")]
        public async Task<ActionResult<ArtListModel>> GetCreatedArt(string username)
        {
            var result = await _artService.GetCreatedArt(username);

            if (!result.Success) return BadRequest(result.Errors);

            return Ok(result.ResponseData);
        }
    }
}