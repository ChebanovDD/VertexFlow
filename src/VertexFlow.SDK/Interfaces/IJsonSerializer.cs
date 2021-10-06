using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Interfaces
{
    public interface IJsonSerializer
    {
        Task<HttpContent> SerializeAsync<T>(T data, CancellationToken cancellationToken);
        Task<T> DeserializeAsync<T>(HttpContent httpContent, CancellationToken cancellationToken);
    }
}