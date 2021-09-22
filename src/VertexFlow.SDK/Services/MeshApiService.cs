using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK.Services
{
    internal class MeshApiService : IMeshApi
    {
        private const string MeshesUri = "api/meshes";
        private const string JsonMediaType = "application/json";
        
        private readonly HttpClient _httpClient;

        public MeshApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string BaseAddress => _httpClient.BaseAddress?.OriginalString;

        public async Task Create<TRequest>(TRequest meshRequest)
        {
            await _httpClient.PostAsync(MeshesUri, GetHttpContent(meshRequest));
        }

        public async Task<TResponse> GetAsync<TResponse>(string meshId)
        {
            var response = await SendGetRequestAsync($"{MeshesUri}/{meshId}");
            return response == null ? default : JsonConvert.DeserializeObject<TResponse>(response);
        }

        public async Task<TResponse[]> GetAllAsync<TResponse>()
        {
            var response = await SendGetRequestAsync(MeshesUri);
            return response == null ? null : JsonConvert.DeserializeObject<TResponse[]>(response);
        }

        public async Task UpdateAsync<TRequest>(string meshId, TRequest meshRequest)
        {
            await _httpClient.PutAsync($"{MeshesUri}/{meshId}", GetHttpContent(meshRequest));
        }

        public async Task DeleteAsync(string meshId)
        {
            await _httpClient.DeleteAsync($"{MeshesUri}/{meshId}");
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        private async Task<string> SendGetRequestAsync(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode == false)
            {
                return null;
            }
            
            return await response.Content.ReadAsStringAsync();
        }

        private StringContent GetHttpContent<T>(T data)
        {
            return new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, JsonMediaType);
        }
    }
}