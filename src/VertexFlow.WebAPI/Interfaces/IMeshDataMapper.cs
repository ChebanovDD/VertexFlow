using VertexFlow.WebAPI.Models;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebAPI.Interfaces
{
    public interface IMeshDataMapper
    {
        Mesh FromRequest(MeshRequest meshRequest);
        MeshResponse ToResponse(Mesh mesh);
    }
}