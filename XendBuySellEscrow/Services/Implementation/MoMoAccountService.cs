using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class MoMoAccountService : IMoMoAccountService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly JsonSerializerOptions _options;
        public MoMoAccountService()
        {
            string baseAddress = "https://sandbox.momodeveloper.mtn.com/v1_0/";
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(baseAddress);
            }
            //_httpClient.BaseAddress = new Uri("https://sandbox.momodeveloper.mtn.com/v1_0/");
            //_httpClient.Timeout = new TimeSpan(0, 0, 30);

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

        public async Task<ApiKeyResponse> GetApiKey(string OcpApimSubscriptionKey, Guid xReferenceId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{OcpApimSubscriptionKey}");
            HttpResponseMessage response = await _httpClient.PostAsync($"apiuser/{xReferenceId}/apikey?" + queryString, null);

            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);

            return new ApiKeyResponse
            {
                ApiResponse = json["apiKey"].ToString()
            };
             
        }

        public async Task<HttpResponseMessage> GetCreatedUser(string OcpApimSubscriptionKey, Guid xReferenceId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{OcpApimSubscriptionKey}");
            _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", $"{xReferenceId}");
            HttpResponseMessage response = await _httpClient.GetAsync($"apiuser/{xReferenceId}");
            return response;
        }

        public async Task<TokenKeyResponse> GenerateApiToken(string OcpApimSubscriptionKey, Guid xReferenceId)

        {
            ApiKeyResponse tokenKey = await GetApiKey(OcpApimSubscriptionKey, xReferenceId);
            string getToken = tokenKey.ApiResponse;
            string basicAuthCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(xReferenceId + ":" + getToken));

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", OcpApimSubscriptionKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + basicAuthCredentials);
            
            HttpResponseMessage response = await _httpClient.PostAsync($"https://sandbox.momodeveloper.mtn.com/collection/token/", null);
            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);

            return new TokenKeyResponse
            {
                ApiToken = json["access_token"].ToString()
            };
        }
    }
}
