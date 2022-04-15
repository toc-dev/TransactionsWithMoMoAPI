using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using XendBuySellEscrow.Models;
using XendBuySellEscrow.Models.ResponseModels;

namespace XendBuySellEscrow.Services.Interfaces
{
    public interface IMoMoAccountService
    {
        Task<HttpResponseMessage> CreateAPIUser(Guid xReferenceId, string OcpApimSubscriptionKey, CallbackModel callbackLink);
        Task<HttpResponseMessage> GetCreatedUser(string OcpApimSubscriptionKey, Guid xReferenceId);
        Task<ApiKeyResponse> GetApiKey(string OcpApimSubscriptionKey, Guid xReferenceId);
        Task<TokenKeyResponse> GenerateApiToken(string OcpApimSubscriptionKey, Guid xReferenceId);
    }
}
