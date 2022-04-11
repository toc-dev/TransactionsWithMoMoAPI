using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XendBuySellEscrow.Models;
using XendBuySellEscrow.Services.Interfaces;

namespace XendBuySellEscrow.Services.Implementation
{
    public class TransactionServices : ITransactionServices
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly JsonSerializerOptions _options;
        public TransactionServices()
        {
            _httpClient.BaseAddress = new Uri("https://sandbox.momodeveloper.mtn.com/collection/v1_0/");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<HttpResponseMessage> CheckIfUserIsRegisteredAndActive(string OcpApimSubscriptionKey, Guid xReferenceId, string xTargetEnvironment = "sandbox")
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Reference-Id", $"{xReferenceId.ToString()}");
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", $"{OcpApimSubscriptionKey}");
            HttpResponseMessage response = await _httpClient.PostAsync($"apiuser/{xReferenceId}/apikey", null);

            return response;
        }
        public async Task<HttpResponseMessage> GetAccountBalance(string OcpApimSubscriptionKey, string token, string xTargetEnvironment = "sandbox")
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{OcpApimSubscriptionKey}");
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", $"{xTargetEnvironment}");
            HttpResponseMessage response = await _httpClient.GetAsync($"account/balance");
            return response;
        }

        public async Task<HttpResponseMessage> GetRequestToPayTransaction(Guid xReferenceId, string OcpApimSubscriptionKey, string token, string xTargetEnvironment = "sandbox")
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", OcpApimSubscriptionKey);
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);
            HttpResponseMessage response = await _httpClient.GetAsync($"requesttopay/{xReferenceId}");
            return response;
        }

        public async Task<HttpResponseMessage> RequestToPay(Guid xReferenceId, string OcpApimSubscriptionKey, PaymentDetails paymentDetails, string token, string xTargetEnvironment = "sandbox")
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token}");
            //_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", $"{xReferenceId}");
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{OcpApimSubscriptionKey}");
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", $"{xTargetEnvironment}");
            

            string json = JsonConvert.SerializeObject(paymentDetails);
            var paymentDetailsAsJson = new StringContent(JsonConvert.SerializeObject(paymentDetails), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"requesttopay", paymentDetailsAsJson);
            return response;
        }
    }
}
