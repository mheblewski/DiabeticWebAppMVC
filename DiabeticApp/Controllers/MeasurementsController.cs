using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DiabeticApp.ApiInfrastructure.Clients.Measurements;
using DiabeticApp.ApiInfrastructure.Models;
using DiabeticApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DiabeticApp.Controllers
{
    public class MeasurementsController : Controller
    {
        private readonly IMeasurementsClient _measurementsClient;

        public MeasurementsController(IMeasurementsClient measurementsClient)
        {
            _measurementsClient = measurementsClient;
        }
    
        [HttpGet]
        public async Task<ActionResult> AllMeasurements()
        {
            List<MeasurementViewModel> model = null;
            var token = GetToken();
            var response = await _measurementsClient.GetAllMeasurementWithHttpResponseUsingToken(token);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                ClearAndAddModelError("No measurements to show.");
            }
            else if (response.IsSuccessStatusCode)
            {
                model = await _measurementsClient.GetMeasurementsFromResponse(response);
            }
            else
            {
                ClearAndAddModelError("Something wrong, try agin later.");
            }

            return View(model);
        }

        public ActionResult AddMeasurement()
        {
            var model = new MeasurementViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddMeasurement(MeasurementViewModel model)
        {
            var token = GetToken();
            var response = await _measurementsClient.AddMeasurementAndGetHttpResponseUsingToken(model, token);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AllMeasurements", "Measurements");
            }

            return View(model);
        }

        //public ActionResult DeleteMeasurement()
        //{
        //    return RedirectToAction("AllMeasurements", "Measurements");
        //}

        public async Task<ActionResult> DeleteMeasurement(int id)
        {
            var token = GetToken();
            var response = await _measurementsClient.DeleteMeasurementAndGetHttpResponse(id, token);
            if (!response.IsSuccessStatusCode)
            {
                ClearAndAddModelError("A problem occurred, please try again later");

            }
            return RedirectToAction("AllMeasurements", "Measurements");
        }


        private string GetToken()
        {
            return HttpContext.Session.GetString("Token");
        }

        private void ClearAndAddModelError(string message)
        {
            ModelState.Clear();
            ModelState.AddModelError("", message);
        }
    }
}