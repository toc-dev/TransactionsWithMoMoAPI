using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using XendBuySellEscrow.Models;

namespace XendBuySellEscrow.Services.Interfaces
{
    public interface ITransactionServices
    {
        Task<HttpResponseMessage> CheckIfUserIsRegisteredAndActive(string OcpApimSubscriptionKey, Guid xReferenceId, string xTargetEnvironment = "sandbox");

        Task<HttpResponseMessage> RequestToPay(Guid xReferenceId, string OcpApimSubscriptionKey, PaymentDetails paymentDetails, string token, string xTargetEnvironment = "sandbox");
        Task<HttpResponseMessage> GetRequestToPayTransaction(Guid xReferenceId, string OcpApimSubscriptionKey, string token, string xTargetEnvironment = "sandbox");
        Task<HttpResponseMessage> GetAccountBalance(string OcpApimSubscriptionKey, string token, string xTargetEnvironment = "sandbox");
    }
}
