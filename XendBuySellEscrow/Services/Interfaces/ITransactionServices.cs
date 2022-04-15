using RestSharp;
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

        Task<string> RequestToPay(Guid xReferenceId, string OcpApimSubscriptionKey, PaymentDetails paymentDetails, string xTargetEnvironment = "sandbox");
        Task<HttpResponseMessage> GetRequestToPayTransaction(Guid xReferenceId, string OcpApimSubscriptionKey, string tokenKey, string xTargetEnvironment = "sandbox");
        Task<HttpResponseMessage> GetAccountBalance(string OcpApimSubscriptionKey, string token, string xTargetEnvironment = "sandbox");
        Task<HttpResponseMessage> RequestToPayDeliveryNotification(Guid xReferenceId, string ocpApimSubscriptionKey, NotificationMessage notificationMessage, string xTargetEnvironment);
        Task<HttpResponseMessage> Transfer(Guid xReferenceId, string ocpApimSubscriptionKey, TransferModel transferModel, string xTargetEnvironment);
        Task<HttpResponseMessage> RequestToWithdraw(Guid xReferenceId, string ocpApimSubscriptionKey, WithdrawalMethod withdrawalMethod, string xTargetEnvironment);
    }
}
