using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VertexFlow.WebApplication.Interfaces.Services
{
    public interface IMeshService
    {
        Task AddAsync(string projectName, string meshId, Stream meshData, CancellationToken cancellationToken);
        Task<Stream> GetAsync(string projectName, string meshId, CancellationToken cancellationToken);
        Task UpdateAsync(string projectName, string meshId, Stream meshData, CancellationToken cancellationToken);
        Task DeleteAsync(string projectName, string meshId, CancellationToken cancellationToken);
    }
}