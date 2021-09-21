using System.Threading.Tasks;

namespace VertexFlow.SDK.Interfaces
{
    public interface IMeshFlow<in TMeshData>
    {
        Task SendAsync(TMeshData mesh);
        Task UpdateAsync(string meshId, TMeshData mesh);
    }
}