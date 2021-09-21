using System.Collections.Generic;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Interfaces
{
    public interface IMeshStore<TMeshData>
    {
        Task<TMeshData> GetAsync(string meshId);
        IAsyncEnumerable<TMeshData> GetAllAsync();
        Task DeleteAsync(string meshId);
    }
}