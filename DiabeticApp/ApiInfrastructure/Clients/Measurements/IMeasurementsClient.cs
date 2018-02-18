using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DiabeticApp.Models;

namespace DiabeticApp.ApiInfrastructure.Clients.Measurements
{
    public interface IMeasurementsClient
    {
        Task<List<MeasurementViewModel>> GetMeasurementsFromResponse(HttpResponseMessage response);
        Task<HttpResponseMessage> GetHttpResponse(string token);
    }
}
