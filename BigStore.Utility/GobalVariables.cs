using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BigStore.Utility
{
    public static class GobalVariables
    {
        private static readonly HttpClient httpClient = new();
        public static HttpClient WebAPIClient = httpClient;

        static GobalVariables()
        {
            WebAPIClient.BaseAddress = new Uri("https://localhost:7292/api/"); // service api gateway
            WebAPIClient.DefaultRequestHeaders.Clear();
            WebAPIClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static void AddAuthorizationHeader(this HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
