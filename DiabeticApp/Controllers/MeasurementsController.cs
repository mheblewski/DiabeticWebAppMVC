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
            var token = HttpContext.Session.GetString("Token");
            var response = await _measurementsClient.GetHttpResponse(token);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                ModelState.Clear();
                ModelState.AddModelError("", "No measurements to show.");
            }
            else if (response.IsSuccessStatusCode)
            {
                model = await _measurementsClient.GetMeasurementsFromResponse(response);
            }
            else
            {
                ModelState.Clear();
                ModelState.AddModelError("", "Something wrong, try agin later.");
            }

            return View(model);
        }
    }
}