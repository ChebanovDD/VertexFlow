using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VertexFlow.SDK.Extensions
{
    internal static class HttpClientExtensions
    {
        private const string JsonMediaType = "application/json";

        public static async Task PostAsJsonAsync<T>(this HttpClient client, string requestUri, T data)
        {
            await client.PostAsync(requestUri, GetStringContent(data)).ConfigureAwait(false);
        }

        public static async Task<T> GetAsObjectAsync<T>(this HttpClient client, string requestUri)
        {
            var response = await client.GetAsync(requestUri, CancellationToken.None).ConfigureAwait(false);
            if (response.IsSuccessStatusCode == false)
            {
                return default;
            }

            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return data == null ? default : JsonConvert.DeserializeObject<T>(data);
        }

        public static async Task PutAsJsonAsync<T>(this HttpClient client, string requestUri, T data)
        {
            await client.PutAsync(requestUri, GetStringContent(data)).ConfigureAwait(false);
        }

        private static StringContent GetStringContent<T>(T data)
        {
            return new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, JsonMediaType);
        }
    }
}