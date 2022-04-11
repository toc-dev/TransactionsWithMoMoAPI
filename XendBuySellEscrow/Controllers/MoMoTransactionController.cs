using Microsoft.AspNetCore.Authorization;
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
    public class MoMoTransactionController : ControllerBase
    {
        private readonly ITransactionServices _transactionService;
        public MoMoTransactionController(ITransactionServices transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpPost("transact")]
        public async Task<IActionResult> RequestToPay([FromHeader] string ocpApimKey, [FromHeader] Guid xReferenceId, [FromBody] PaymentDetails paymentDetails, [FromHeader] string token, [FromHeader] string xTargetEnvironment)
        {
            HttpResponseMessage response = await _transactionService.RequestToPay(xReferenceId, ocpApimKey, paymentDetails, token, xTargetEnvironment);
            return Ok(response);
        }

        [HttpGet("{xReferenceId}")]
        public async Task<IActionResult> GetRequestToPayTransaction(Guid xReferenceId, string OcpApimSubscriptionKey, string token)
        {
            HttpResponseMessage response = await _transactionService.GetRequestToPayTransaction(xReferenceId, OcpApimSubscriptionKey, token);
            return Ok(response);
        }

        [HttpGet("chechkuser")]
        public async Task<IActionResult> CheckIfUserIsRegisteredAndActive(string OcpApimSubscriptionKey, Guid xReferenceId, string xTargetEnvironment = "sandbox")
        {
            HttpResponseMessage responseMessage = await _transactionService.CheckIfUserIsRegisteredAndActive(OcpApimSubscriptionKey, xReferenceId, xTargetEnvironment);
            return Ok(responseMessage);
        }
        [HttpGet("accountbalance")]
        public async Task<IActionResult> GetAccountBalance(string OcpApimSubscriptionKey, string token, string xTargetEnvironment = "sandbox")
        {
            HttpResponseMessage responseMessage = await _transactionService.GetAccountBalance(OcpApimSubscriptionKey, token, xTargetEnvironment);
            return Ok(responseMessage);
        }
    }
}
