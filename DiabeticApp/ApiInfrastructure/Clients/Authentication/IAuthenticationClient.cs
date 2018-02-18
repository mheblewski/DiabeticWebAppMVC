using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DiabeticApp.ApiInfrastructure.Models;
using DiabeticApp.Models;

namespace DiabeticApp.ApiInfrastructure.Clients.Authentication
{
    public interface IAuthenticationClient
    {
        Task<HttpResponseMessage> GetHttpResponse(LoginViewModel model);
        Task<TokenModel> GetTokenFromHttpResponse(HttpResponseMessage response);
    }
}
