using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Songer.Core.Client
{
    public interface IRestClient
    {
        HttpRequestHeaders DefaultRequestHeaders { get; }
        Uri BaseAddress { get; set; }

        Task<HttpResponseMessage> GetAsync(string requestUri);

        Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T data);
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);

        Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T data);
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content);

        Task<HttpResponseMessage> DeleteAsync(string requestUri);

        void Reset();
    }
}
