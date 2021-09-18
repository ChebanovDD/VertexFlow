using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using VertexFlow.Contracts.Requests;
using VertexFlow.Contracts.Responses;

namespace VertexFlow.SDK.Interfaces
{
    public interface IMeshesApi
    {
        [Post("/api/meshes")]
        Task<ApiResponse<MeshResponse>> Create([Body] MeshRequest meshRequest);

        [Get("/api/meshes/{meshId}")]
        Task<ApiResponse<MeshResponse>> GetAsync(string meshId);

        [Get("/api/meshes")]
        Task<ApiResponse<IEnumerable<MeshResponse>>> GetAllAsync();
        
        [Put("/api/meshes/{meshId}")]
        Task<ApiResponse<string>> UpdateAsync(string meshId, [Body] MeshRequest meshRequest);
        
        [Delete("/api/meshes/{meshId}")]
        Task<ApiResponse<string>> DeleteAsync(string meshId);
    }
}