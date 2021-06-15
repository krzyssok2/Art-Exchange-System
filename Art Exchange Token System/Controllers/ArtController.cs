using Art_Exchange_Token_System.Entities;
using Art_Exchange_Token_System.Models;
using Art_Exchange_Token_System.Models.RequestModels;
using Art_Exchange_Token_System.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ArtExchangeContext _context;
        private readonly ArtService _artService;
        public ArtController(UserManager<IdentityUser> identityService, ArtExchangeContext artExchangeContext, ArtService artService)
        {
            _userManager = identityService;
            _context = artExchangeContext;
            _artService = artService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ArtDataCreationModel>> PostArt([FromForm] PostArtModel request)
        {
            var email = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;

            var user = await _userManager.FindByEmailAsync(email);

            var userdata = _context.UserData
                .First(i => i.IdentityUser.Id == user.Id);

            if (!(request.File.Length > 0)) return BadRequest("No File");

            string path = Directory.GetCurrentDirectory() + "\\uploads\\";

            _artService.DirectoryCreationCheck(path);

            string guid = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString();

            string extension = request.File.FileName[(request.File.FileName.LastIndexOf('.') + 1)..];

            _artService.SaveImageToDisk(path + guid + "." + extension, request.File);

            var category = _context.ArtCategories.FirstOrDefault(i => i.CategoryName == request.Category);

            if (category == null)
            {
                category = _context.ArtCategories.First(i => i.CategoryName == "Default");
            }

            var art = new ArtData
            {
                Catgegory = category,
                CurrentOwner = userdata,
                Description = request.Description,
                FileName = guid + "." + extension,
                Name = request.Name,
                OriginalCreator = userdata
            };

            _context.ArtData.Add(art);
            userdata.OwnedArt.Add(art);

            _context.SaveChanges();

            var artDB = _context.ArtData.Include(i => i.Catgegory).First(i => i.FileName == guid + "." + extension);

            return Ok(new ArtDataCreationModel
            {
                Catgegory = new ArtCategoryModel
                {
                    Id = artDB.Catgegory.Id,
                    Name = artDB.Catgegory.CategoryName
                },
                Description = artDB.Description,
                FileName = guid + "." + extension,
                Id = artDB.Id,
                Name = artDB.Name
            });
        }

        [HttpDelete("{fileName}")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteArt(string fileName)
        {
            var email = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;

            var user = await _userManager.FindByEmailAsync(email);

            var image = _context.ArtData
                .Include(i => i.CurrentOwner)
                .FirstOrDefault(i => i.FileName == fileName);

            if (image == null) return BadRequest("Not found");

            if (!(await _userManager.IsInRoleAsync(user, "admin") 
                || image.CurrentOwner.DisplayName == user.UserName))
                return BadRequest("Permission denied");

            System.IO.File.Delete(Directory.GetCurrentDirectory() + "\\uploads\\" + fileName);

            _context.ArtData.Remove(image);
            _context.SaveChanges();            
            
            return Ok("Deleted");

        }

        [HttpGet("{fileName}")]
        public async Task<ActionResult> GetPicture(string fileName)
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "\\uploads\\";
            string filepath = path + fileName;

            if (System.IO.File.Exists(filepath))
            {
                byte[] b = System.IO.File.ReadAllBytes(filepath);

                string extension = fileName[(fileName.IndexOf('.') + 1)..];

                return File(b, "image/" + extension);
            }

            return BadRequest("Not Found");
        }

        [HttpGet("owned/{username}")]
        public async Task<ActionResult<OwnedArtDataModel>> GetOwnedArt(string username)
        {
            var art = _context.ArtData
                .Include(i => i.CurrentOwner)
                .Include(i => i.Catgegory)
                .Where(i => i.CurrentOwner.DisplayName == username);

            var owned = art.Select(i => new OwnedArtDataModel
            {
                Id = i.Id,
                Name = i.Name,
                FileName = i.FileName,
                Description = i.Description,
                Catgegory = new ArtCategoryModel
                {
                    Id = i.Catgegory.Id,
                    Name = i.Catgegory.CategoryName
                }
            });

            return Ok(owned);
        }

        [HttpGet("created/{username}")]
        public async Task<ActionResult<OwnedArtDataModel>> GetCreatedArt(string username)
        {
            var art = _context.ArtData
                .Include(i => i.OriginalCreator)
                .Include(i => i.Catgegory)
                .Where(i => i.OriginalCreator.DisplayName == username);

            var owned = art.Select(i => new OwnedArtDataModel
            {
                Id = i.Id,
                Name = i.Name,
                FileName = i.FileName,
                Description = i.Description,
                Catgegory = new ArtCategoryModel
                {
                    Id = i.Catgegory.Id,
                    Name = i.Catgegory.CategoryName
                }
            });

            return Ok(owned);
        }
    }
}