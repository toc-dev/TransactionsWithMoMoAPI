using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using XendBuySellEscrow.Models;

namespace XendBuySellEscrow.Services.Interfaces
{
    public interface IMoMoAccountService
    {
        Task<HttpResponseMessage> CreateAPIUser(Guid xReferenceId, string OcpApimSubscriptionKey, CallbackModel callbackLink);
        Task<HttpResponseMessage> GetCreatedUser(string OcpApimSubscriptionKey, Guid xReferenceId);
        Task<HttpResponseMessage> GetApiKey(string OcpApimSubscriptionKey, Guid xReferenceId);
        Task<HttpResponseMessage> GenerateApiToken(string OcpApimSubscriptionKey, Guid xReferenceId, string tokenKey);
    }
}
