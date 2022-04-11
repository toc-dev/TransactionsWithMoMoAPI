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
using XendBuySellEscrow.Services.Interfaces;

namespace XendBuySellEscrow.Services.Implementation
{
    public class MoMoAccountService : IMoMoAccountService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly JsonSerializerOptions _options;
        public MoMoAccountService()
        {

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.BaseAddress = new Uri("https://sandbox.momodeveloper.mtn.com/v1_0/");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<HttpResponseMessage> CreateAPIUser(Guid xReferenceId, string OcpApimSubscriptionKey, CallbackModel callbackLink)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{OcpApimSubscriptionKey}");
            _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", $"{xReferenceId}");
            
            var content = new StringContent(JsonConvert.SerializeObject(callbackLink), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"apiuser", content);
            return response;
        }

        public async Task<HttpResponseMessage> GetApiKey(string OcpApimSubscriptionKey, Guid xReferenceId)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{OcpApimSubscriptionKey}");
            HttpResponseMessage response = await _httpClient.PostAsync($"apiuser/{xReferenceId}/apikey?" + queryString, null);
            return response;
             
        }

        public async Task<HttpResponseMessage> GetCreatedUser(string OcpApimSubscriptionKey, Guid xReferenceId)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{OcpApimSubscriptionKey}");
            _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", $"{xReferenceId}");
            HttpResponseMessage response = await _httpClient.GetAsync($"apiuser/{xReferenceId}");
            return response;
        }

        public async Task<HttpResponseMessage> GenerateApiToken(string OcpApimSubscriptionKey, Guid xReferenceId, string tokenKey)

        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenKey}");
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", OcpApimSubscriptionKey);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", $"{xReferenceId}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", tokenKey);
            HttpResponseMessage response = await _httpClient.PostAsync($"collection/token", null);
            return response;
        }
    }
}
