using System.Threading.Tasks;
using Refit;

namespace VertexFlow.SDK.Interfaces
{
    internal interface IMeshesApi
    {
        [Post("/api/meshes")]
        Task<ApiResponse<TResponse>> Create<TRequest, TResponse>([Body] TRequest meshRequest);

        [Get("/api/meshes/{meshId}")]
        Task<ApiResponse<TResponse>> GetAsync<TResponse>(string meshId);

        [Get("/api/meshes")]
        Task<ApiResponse<TResponse[]>> GetAllAsync<TResponse>();
        
        [Put("/api/meshes/{meshId}")]
        Task<ApiResponse<string>> UpdateAsync<TRequest>(string meshId, [Body] TRequest meshRequest);
        
        [Delete("/api/meshes/{meshId}")]
        Task<ApiResponse<string>> DeleteAsync(string meshId);
    }
}