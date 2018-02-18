using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DiabeticApp.ApiInfrastructure.Models;
using DiabeticApp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DiabeticApp.ApiInfrastructure.Clients.Authentication
{
    public class AuthenticationClient : BaseClient, IAuthenticationClient
    {
        private readonly HttpClient _httpClient;
        private readonly string tokenUrl = "token";
        public AuthenticationClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> GetHttpResponse(LoginViewModel model)
        {
            var dictionary = CreateDictionaryFromLoginModelAndAddGrantType(model, "password");
            var content = new FormUrlEncodedContent(dictionary);
            var response = await _httpClient.PostAsync(tokenUrl, content);
            return response;
        }

        public async Task<TokenModel> GetTokenFromHttpResponse(HttpResponseMessage response)
        {
            var jsonMessage = await GetJsonMessage(response);

            TokenModel tokenResponse = (TokenModel)JsonConvert.DeserializeObject
                (jsonMessage, typeof(TokenModel));
            return tokenResponse;
        }

        private IDictionary<string,string> CreateDictionaryFromLoginModelAndAddGrantType
            (LoginViewModel model, string grantType)
        {
            Helpers helpers = new Helpers();
            var dictionary = helpers.ObjectToDictionary(model);
            dictionary.Add("grant_type", grantType);
            return dictionary;
        }
    }
}
