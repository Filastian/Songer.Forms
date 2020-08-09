using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Songer.Core.Client
{
    public class RestClient : HttpClient, IRestClient
    {
        public RestClient(Uri rootAppPath)
        {
            BaseAddress = rootAppPath;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T data)
        {
            return await PostAsync(BaseAddress + url, CreateJsonContent(data));
        }

        public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T data)
        {
            return await PutAsync(BaseAddress + url, CreateJsonContent(data));
        }

        public void Reset()
        {
            DefaultRequestHeaders.Authorization = null;
        }

        private StringContent CreateJsonContent<T>(T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
    }
}
