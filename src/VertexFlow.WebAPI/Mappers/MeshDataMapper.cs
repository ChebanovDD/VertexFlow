using VertexFlow.Contracts.Requests;
using VertexFlow.Contracts.Responses;
using VertexFlow.WebAPI.Interfaces;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebAPI.Mappers
{
    public class MeshDataMapper : IMeshDataMapper
    {
        public Mesh FromRequest(MeshRequest meshRequest)
        {
            return new Mesh(meshRequest.Id, meshRequest.Triangles, meshRequest.Vertices, meshRequest.Normals);
        }

        public MeshResponse ToResponse(Mesh mesh)
        {
            return new MeshResponse
            {
                Id = mesh.Id,
                Triangles = mesh.Triangles,
                Vertices = mesh.Vertices,
                Normals = mesh.Normals
            };
        }
    }
}