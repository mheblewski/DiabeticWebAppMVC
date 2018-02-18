using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DiabeticApp.ApiInfrastructure.Clients.Authentication;
using DiabeticApp.ApiInfrastructure.Models;
using DiabeticApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiabeticApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationClient _authenticationClient;

        public AccountController(IAuthenticationClient authenticationClient)
        {
            _authenticationClient = authenticationClient;
        }

        public ActionResult Login()
        {
            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (model.Username == null || model.Password == null)
            {
                ClearAndAddModelError("Please enter username and password.");
                return View(model);
            }
            var response = await _authenticationClient.GetHttpResponse(model);
            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await _authenticationClient.GetTokenFromHttpResponse(response);
                SetToken(tokenResponse.AccessToken);
                return RedirectToAction("AllMeasurements", "Measurements");
            }

            ModelState.Clear();
            ModelState.AddModelError("", "The username or password is incorrect");
            return View(model);
        }

        private void SetToken(string token)
        {
            HttpContext.Session.SetString("Token", token);
        }

        private void ClearAndAddModelError(string message)
        {
            ModelState.Clear();
            ModelState.AddModelError("", message);
        }
    }
}