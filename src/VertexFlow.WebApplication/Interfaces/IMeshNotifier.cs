using System.Threading.Tasks;

namespace VertexFlow.WebApplication.Interfaces
{
    public interface IMeshNotifier
    {
        Task Update(int meshId);
    }
}