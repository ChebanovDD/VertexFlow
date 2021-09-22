using System.Threading.Tasks;

namespace VertexFlow.SDK.Interfaces
{
    public interface IMeshStore<TMeshData>
    {
        Task<TMeshData> GetAsync(string meshId);
        Task<TMeshData[]> GetAllAsync();
        Task DeleteAsync(string meshId);
    }
}