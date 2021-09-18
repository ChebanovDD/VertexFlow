using VertexFlow.Contracts.Requests;
using VertexFlow.Contracts.Responses;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebAPI.Interfaces
{
    public interface IMeshDataMapper
    {
        Mesh FromRequest(MeshRequest meshRequest);
        MeshResponse ToResponse(Mesh mesh);
    }
}