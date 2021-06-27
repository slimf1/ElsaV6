using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ElsaV6.Utils
{
    public class Http
    {
        private static readonly HttpClient HTTP_CLIENT = new HttpClient();

        public static async Task<string> PostAsync(string url, IDictionary<string, string> postBody)
        {
            var content = new FormUrlEncodedContent(postBody);
            var response = await HTTP_CLIENT.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
