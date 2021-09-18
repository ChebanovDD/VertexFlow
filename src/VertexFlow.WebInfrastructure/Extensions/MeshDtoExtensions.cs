using VertexFlow.WebApplication.Models;
using VertexFlow.WebInfrastructure.DTOs;

namespace VertexFlow.WebInfrastructure.Extensions
{
    internal static class MeshDtoExtensions
    {
        public static MeshDto ToDto(this Mesh mesh)
        {
            return new MeshDto
            {
                Id = mesh.Id,
                Triangles = mesh.Triangles,
                Vertices = mesh.Vertices,
                Normals = mesh.Normals
            };
        }

        public static Mesh ToMesh(this MeshDto meshData)
        {
            return new Mesh
            (
                meshData.Id,
                meshData.Triangles,
                meshData.Vertices,
                meshData.Normals
            );
        }
    }
}