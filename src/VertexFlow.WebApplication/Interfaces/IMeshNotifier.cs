using System.Threading;
using System.Threading.Tasks;

namespace VertexFlow.WebApplication.Interfaces
{
    public interface IMeshNotifier
    {
        Task Created(string projectName, string meshId, CancellationToken cancellationToken);
        Task Updated(string projectName, string meshId, CancellationToken cancellationToken);
    }
}