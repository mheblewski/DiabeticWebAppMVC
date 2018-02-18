using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DiabeticApp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DiabeticApp.ApiInfrastructure.Clients.Measurements
{
    public class MeasurementsClient : BaseClient, IMeasurementsClient
    {
        private readonly HttpClient _httpClient;
        private readonly string measurementsUrl = "api/measurements";

        public MeasurementsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MeasurementViewModel>> GetMeasurementsFromResponse(HttpResponseMessage response)
        {
            List<MeasurementViewModel> measurementsList = null;
            
            var jsonMessage = await GetJsonMessage(response);
            measurementsList = (List<MeasurementViewModel>)JsonConvert.DeserializeObject(jsonMessage, typeof(List<MeasurementViewModel>));
            measurementsList.Reverse();

            return measurementsList;
        }

        public async Task<HttpResponseMessage> GetHttpResponse(string token)
        {
            AddTokenToAuthorizationHeader(token);
            var response = await _httpClient.GetAsync(measurementsUrl);
            return response;
        }

        private void AddTokenToAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
