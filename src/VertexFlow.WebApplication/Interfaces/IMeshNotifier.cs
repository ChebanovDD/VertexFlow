using System.Threading;
using System.Threading.Tasks;

namespace VertexFlow.WebApplication.Interfaces
{
    public interface IMeshNotifier
    {
        Task Created(string meshId, CancellationToken cancellationToken);
        Task Updated(string meshId, CancellationToken cancellationToken);
    }
}