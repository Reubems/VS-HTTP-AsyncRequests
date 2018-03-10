using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BrowserAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            //string res = getResultAsync().Result;
            // Console.WriteLine(res);

            //PostResultAsync();

            int result = GetNumLoadBalancersAsync().Result;
            Console.WriteLine(result);


         }

        static async Task<string> getResultAsync()
        {
            string result = await Utilities.Get("");
            return result ;
        }
        static void  PostResultAsync()
        {
            Utilities.Post("").Wait();
        }

        static async Task<int> GetNumLoadBalancersAsync()
        {
            var _httpClient = new HttpClient();

            using (var result = await _httpClient.GetAsync("https://browserapinumloadbal.azurewebsites.net/api/GetNumLoadBalancers").ConfigureAwait(false))
            {
                string content = await result.Content.ReadAsStringAsync();
                return int.Parse(content);
            }
        }
    }
}
