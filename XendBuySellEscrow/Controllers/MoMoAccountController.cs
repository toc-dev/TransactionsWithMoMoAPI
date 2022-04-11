using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using XendBuySellEscrow.Models;
using XendBuySellEscrow.Services.Interfaces;

namespace XendBuySellEscrow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MoMoAccountController : ControllerBase
    {
        private readonly IMoMoAccountService _moMoAccountService;
        public MoMoAccountController(IMoMoAccountService moMoAccountService)
        {
            _moMoAccountService = moMoAccountService;
        }
        [HttpPost("{id}/apikey")]
        public async Task<IActionResult> GetApiKey(string ocpApimKey, Guid id)
        {
            HttpResponseMessage apiKey = await  _moMoAccountService.GetApiKey(ocpApimKey, id);
            return (Ok(apiKey));
        }
        [HttpPost("createuser")]
        public async Task<IActionResult> CreateApiUser(string ocpApimKey, Guid id, CallbackModel callbackLink)
        {
            HttpResponseMessage responseMessage = await _moMoAccountService.CreateAPIUser(id, ocpApimKey, callbackLink);
            return (Ok(responseMessage));
        }
        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser(string OcpApimSubscriptionKey, Guid xReferenceId)
        {
            HttpResponseMessage responseMessage = await _moMoAccountService.GetCreatedUser(OcpApimSubscriptionKey, xReferenceId);
            return Ok(responseMessage);
        }
        [HttpPost("gettoken")]
        public async Task<IActionResult> GenerateToken(string OcpApimSubscriptionKey, Guid xReferenceId, string tokenKey)
        {
            HttpResponseMessage responseMessage = await _moMoAccountService.GenerateApiToken(OcpApimSubscriptionKey, xReferenceId, tokenKey);
            return Ok(responseMessage);
        }
    }
}
