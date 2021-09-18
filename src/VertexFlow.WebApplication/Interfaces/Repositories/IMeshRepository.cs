using System.Collections.Generic;
using System.Threading.Tasks;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebApplication.Interfaces.Repositories
{
    public interface IMeshRepository
    {
        Task AddAsync(Mesh mesh);
        Task<Mesh> GetAsync(string meshId);
        IAsyncEnumerable<Mesh> GetAllAsync();
        Task UpdateAsync(string meshId, Mesh newMesh);
        Task DeleteAsync(string meshId);
    }
}