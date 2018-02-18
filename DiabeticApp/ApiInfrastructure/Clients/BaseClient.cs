using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiabeticApp.ApiInfrastructure.Clients
{
    public class BaseClient
    {
        protected async Task<string> GetJsonMessage(HttpResponseMessage response)
        {
            string jsonMessage;
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                jsonMessage = new StreamReader(responseStream).ReadToEnd();
            }

            return jsonMessage;
        }
    }
}
