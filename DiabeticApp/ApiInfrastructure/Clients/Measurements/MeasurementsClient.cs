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

        public async Task<HttpResponseMessage> GetAllMeasurementWithHttpResponseUsingToken(string token)
        {
            AddTokenToAuthorizationHeader(token);
            var response = await _httpClient.GetAsync(measurementsUrl);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteMeasurementAndGetHttpResponse(int measurementId, string token)
        {
            AddTokenToAuthorizationHeader(token);
            var response = await _httpClient.DeleteAsync(measurementsUrl + "/" + measurementId);
            return response;
        }

        public async Task<HttpResponseMessage> AddMeasurementAndGetHttpResponseUsingToken(MeasurementViewModel model, string token)
        {
            AddTokenToAuthorizationHeader(token);
            var myContent = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = await _httpClient.PostAsync(measurementsUrl, byteContent);
            return result;
        }

        private void AddTokenToAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
