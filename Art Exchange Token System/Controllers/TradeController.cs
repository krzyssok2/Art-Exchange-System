using LOGIC.Interfaces;
using LOGIC.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static DATA.Enums.TradeTransactionStatusEnum;

namespace Art_Exchange_Token_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TradeController : Controller
    {
        private readonly ITradeService _tradeService;
        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(AllOnGoingTradesModel), 200)]
        public async Task<ActionResult<AllOnGoingTradesModel>> GetAllUserTrades()
        {
            var email = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;

            var result = await _tradeService.GetAllUserTradesByEmail(email);

            return Ok(result.ResponseData);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetTradeInfoModel), 200)]
        public async Task<ActionResult<GetTradeInfoModel>> CreateTrade(TradeCreationModel tradeCreationModel)
        {
            var email = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;

            var result = await _tradeService.PostNewTradeAsync(email, tradeCreationModel);

            if (!result.Success) return BadRequest(result.Errors);

            return Ok(result.ResponseData);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        public async Task<ActionResult<GetTradeInfoModel>> GetTrade(long id)
        {
            var email = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;

            var result = await _tradeService.GetTradeInfoById(email, id);

            if (!result.Success) return BadRequest(result.Errors);

            return Ok(result.ResponseData);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrade(long id)
        {
            var email = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;

            var result = await _tradeService.DeleteTradeByIdAsync(email, id);

            if (!result.Success) return BadRequest(result.Errors);

            return Ok();
        }

        [HttpPatch("{id}/StatusChange")]
        public async Task<ActionResult> ChangeTradeStatus(long id, TradeStatus tradeStatus)
        {
            var email = User.Claims.Single(a => a.Type == ClaimTypes.Email).Value;

            var result = await _tradeService.ChangeTradeStatusAsync(email, id, tradeStatus);

            if (!result.Success) return BadRequest(result.Errors);

            return Ok();

        }
    }
}