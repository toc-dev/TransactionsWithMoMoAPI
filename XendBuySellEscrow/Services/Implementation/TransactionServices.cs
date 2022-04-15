using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using XendBuySellEscrow.Models;
using XendBuySellEscrow.Models.ResponseModels;
using XendBuySellEscrow.Services.Interfaces;

namespace XendBuySellEscrow.Services.Implementation
{
    public class TransactionServices : ITransactionServices
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly JsonSerializerOptions _options;
        private readonly IMoMoAccountService _moMoAccountService;
        public TransactionServices(IMoMoAccountService moMoAccountService)
        {
            _moMoAccountService = moMoAccountService;
            string baseAddress = "https://sandbox.momodeveloper.mtn.com/collection/v1_0/";

            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(baseAddress);
            }
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<HttpResponseMessage> CheckIfUserIsRegisteredAndActive(string OcpApimSubscriptionKey, Guid xReferenceId, string xTargetEnvironment = "sandbox")
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Reference-Id", $"{xReferenceId}");
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
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", OcpApimSubscriptionKey);
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);
            HttpResponseMessage response = await _httpClient.GetAsync($"requesttopay/{xReferenceId}");
            return response;
        }

        public async Task<HttpResponseMessage> RequestToPayDeliveryNotification(Guid xReferenceId, string ocpApimSubscriptionKey, NotificationMessage notificationMessage, string xTargetEnvironment)
        {
            TokenKeyResponse bearerToken = await _moMoAccountService.GenerateApiToken(ocpApimSubscriptionKey, xReferenceId);
            string bearerTokenString = bearerToken.ApiToken;
            /*
            ApiKeyResponse tokenKey = await _moMoAccountService.GetApiKey(ocpApimSubscriptionKey, xReferenceId);
            string getToken = tokenKey.ApiResponse;
            string basicAuthCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(xReferenceId + ":" + getToken));
            */

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("referenceId", $"{xReferenceId}");
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{ocpApimSubscriptionKey}");
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", $"{xTargetEnvironment}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{bearerTokenString}");
            string json = JsonConvert.SerializeObject(notificationMessage);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"requesttopay/{xReferenceId}/deliverynotification", content);
            return response;

        }

        public async Task<string> RequestToPay(Guid xReferenceId, string ocpApimSubscriptionKey, PaymentDetails paymentDetails, string xTargetEnvironment = "sandbox")
        {
            TokenKeyResponse bearerToken = await _moMoAccountService.GenerateApiToken(ocpApimSubscriptionKey, xReferenceId);
            string bearerTokenString = bearerToken.ApiToken;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{bearerTokenString}");
            _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", $"{xReferenceId}");
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{ocpApimSubscriptionKey}");
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", $"{xTargetEnvironment}");
            string json = JsonConvert.SerializeObject(paymentDetails);
            var paymentDetailsAsJson = new StringContent(json, Encoding.UTF8, "application/json");
            string uri = _httpClient.BaseAddress + $"requesttopay";
            HttpResponseMessage response = await _httpClient.PostAsync(uri, paymentDetailsAsJson);
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        public async Task<HttpResponseMessage> Transfer(Guid xReferenceId, string ocpApimSubscriptionKey, TransferModel transferModel, string xTargetEnvironment)
        {
            string uri = $"https://sandbox.momodeveloper.mtn.com/disbursement/v1_0/transfer/{xReferenceId}";
            TokenKeyResponse bearerToken = await _moMoAccountService.GenerateApiToken(ocpApimSubscriptionKey, xReferenceId);
            string bearerTokenString = bearerToken.ApiToken;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{bearerTokenString}");
            _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", $"{xReferenceId}");
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{ocpApimSubscriptionKey}");
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", $"{xTargetEnvironment}");
            string json = JsonConvert.SerializeObject(transferModel);
            var transferDetailsAsJson = new StringContent(json, Encoding.UTF8, "application/json");
            //string uri = _httpClient.BaseAddress + $"requesttopay";
            HttpResponseMessage response = await _httpClient.PostAsync(uri, transferDetailsAsJson);
            return response;
        }

        public async Task<HttpResponseMessage> RequestToWithdraw(Guid xReferenceId, string ocpApimSubscriptionKey, WithdrawalMethod withdrawalMethod, string xTargetEnvironment)
        {
            TokenKeyResponse bearerToken = await _moMoAccountService.GenerateApiToken(ocpApimSubscriptionKey, xReferenceId);
            string bearerTokenString = bearerToken.ApiToken;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{bearerTokenString}");
            _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", $"{xReferenceId}");
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{ocpApimSubscriptionKey}");
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", $"{xTargetEnvironment}");
            string json = JsonConvert.SerializeObject(withdrawalMethod);
            var paymentDetailsAsJson = new StringContent(json, Encoding.UTF8, "application/json");
            string uri = _httpClient.BaseAddress + $"requesttopay";
            HttpResponseMessage response = await _httpClient.PostAsync(uri, paymentDetailsAsJson);
            return response;
        }
    }
}
