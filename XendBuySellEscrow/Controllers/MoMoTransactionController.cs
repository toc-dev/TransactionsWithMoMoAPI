using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
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
        public async Task<IActionResult> RequestToPay([FromHeader] string ocpApimKey, [FromHeader] Guid xReferenceId, [FromBody] PaymentDetails paymentDetails, [FromHeader] string xTargetEnvironment)
        {
            string response = await _transactionService.RequestToPay(xReferenceId, ocpApimKey, paymentDetails, xTargetEnvironment);
            return Ok(response);
        }

        [HttpGet("{xReferenceId}")]
        public async Task<IActionResult> GetRequestToPayTransaction(Guid xReferenceId, string OcpApimSubscriptionKey, string tokenKey)
        {
            HttpResponseMessage response = await _transactionService.GetRequestToPayTransaction(xReferenceId, OcpApimSubscriptionKey, tokenKey);
            return Ok(response);
        }

        [HttpGet("checkuser")]
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

        [HttpPost("deliverynotif")]
        public async Task<IActionResult> RequestToPayDeliveryNotification(Guid xReferenceId, string ocpApimSubscriptionKey, NotificationMessage notificationMessage, string xTargetEnvironment)
        {
            HttpResponseMessage responseMessage = await _transactionService.RequestToPayDeliveryNotification(xReferenceId, ocpApimSubscriptionKey, notificationMessage, xTargetEnvironment);
            return Ok(responseMessage);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferFunds([FromHeader] string ocpApimKey, [FromHeader] Guid xReferenceId, [FromBody] TransferModel transferModel, [FromHeader] string xTargetEnvironment)
        {
            HttpResponseMessage response = await _transactionService.Transfer(xReferenceId, ocpApimKey, transferModel, xTargetEnvironment);
            return Ok(response);
        }
        
        [HttpPost("withdrawal")]
        public async Task<IActionResult> WithdrawalRequest([FromHeader] string ocpApimKey, [FromHeader] Guid xReferenceId, [FromBody] WithdrawalMethod withdrawalMethod, [FromHeader] string xTargetEnvironment)
        {
            HttpResponseMessage response = await _transactionService.RequestToWithdraw(xReferenceId, ocpApimKey, withdrawalMethod, xTargetEnvironment);
            return Ok(response);
        }
    }
}
