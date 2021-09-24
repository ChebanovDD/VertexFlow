using System.Threading.Tasks;

namespace VertexFlow.WebApplication.Interfaces
{
    public interface IMeshNotifier
    {
        Task Created(string meshId);
        Task Updated(string meshId);
    }
}