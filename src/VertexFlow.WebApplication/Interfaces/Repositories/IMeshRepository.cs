using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.WebApplication.Enums;

namespace VertexFlow.WebApplication.Interfaces.Repositories
{
    public interface IMeshRepository
    {
        Task AddAsync(string projectName, string meshId, Stream meshData, CancellationToken token);
        Task<Stream> GetAsync(string projectName, string meshId, CancellationToken token);
        Task<MeshStatusCode> UpdateAsync(string projectName, string meshId, Stream meshData, CancellationToken token);
        Task DeleteAsync(string projectName, string meshId, CancellationToken token);
    }
}