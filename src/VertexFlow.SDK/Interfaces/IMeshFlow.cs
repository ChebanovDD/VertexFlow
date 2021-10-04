using System.Threading;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Interfaces
{
    public interface IMeshFlow<in TMeshData>
    {
        Task SendAsync(TMeshData mesh, CancellationToken cancellationToken = default);
        Task UpdateAsync(string meshId, TMeshData mesh, CancellationToken cancellationToken = default);
    }
}