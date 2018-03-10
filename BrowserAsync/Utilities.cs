using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BrowserAsync
{
    public class Utilities
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<string> Get(string queryString)
        {
     
            string url = "http://browserapigeneric20180307062830.azurewebsites.net/api/Terms";

            //Get method
            using (var result = await _httpClient.GetAsync($"{url}{queryString}").ConfigureAwait(false))
            {
                string content = await result.Content.ReadAsStringAsync();
                return content;
            }
        }

       

        public static async Task Post(string postData)
        {

            string url = "http://browserapigeneric20180307062830.azurewebsites.net/api/Terms";

            string str = "{\"Value\": \"sample test 123\"}";
            
            _httpClient.DefaultRequestHeaders
             .Accept
             .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Put method with error handling
            using (var content = new StringContent(str, Encoding.UTF8, "application/json"))
            {
                var result = await _httpClient.PostAsync($"{url}", content).ConfigureAwait(false);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return;
                }
                else
                {
                    // Something wrong happened
                    string resultContent = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    // ... post to Monitor
                }
            }
        }
    }
}
