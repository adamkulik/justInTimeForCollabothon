using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OverpassTurboHandler
{
    public class Level2PostRequest
    {
        public string EndpointAddress { get; set; }
        public string Port { get; set; }
        private readonly HttpClient _httpClient;
        public Level2PostRequest(string endpointAddress, string port)
        {
            EndpointAddress = endpointAddress;
            Port = port;
            _httpClient = new HttpClient();
        }

        public string GetCountryBoundaries(params string[] countryNames)
        {
            var requestDict = new Dictionary<string, string>();

            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("[out:json];\n(");
            foreach (string countryName in countryNames)
            {
                queryBuilder.Append("rel[admin_level=2][\"ISO3166-1\"=\"" + countryName + "\"];");
            }
            queryBuilder.Append(");out geom;");
            requestDict.Add("data", queryBuilder.ToString());
            var requestContent = new FormUrlEncodedContent(requestDict);
            Uri endpointUri = new Uri("http://" + EndpointAddress + ":" + Port + "/api/interpreter");
            var response = _httpClient.PostAsync(endpointUri, requestContent).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
