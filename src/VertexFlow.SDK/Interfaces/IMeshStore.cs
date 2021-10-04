using System.Threading;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Interfaces
{
    public interface IMeshStore<TMeshData>
    {
        Task<TMeshData> GetAsync(string meshId, CancellationToken cancellationToken = default);
        Task<TMeshData[]> GetAllAsync(); // TODO: Change to IAsyncEnumerable with ASP.NET Core 6.0
        Task DeleteAsync(string meshId, CancellationToken cancellationToken = default);
    }
}