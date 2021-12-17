using VertexFlow.WebApplication.Models;
using VertexFlow.WebInfrastructure.DTOs;

namespace VertexFlow.WebInfrastructure.Extensions
{
    internal static class MeshDtoExtensions
    {
        public static MeshDto ToDto(this Mesh mesh, string meshGuid)
        {
            return new MeshDto
            {
                Guid = meshGuid,
                Triangles = mesh.Triangles,
                Vertices = mesh.Vertices,
                Normals = mesh.Normals
            };
        }

        public static Mesh ToMesh(this MeshDto meshData, string meshId)
        {
            return new Mesh
            (
                meshId,
                meshData.Triangles,
                meshData.Vertices,
                meshData.Normals
            );
        }
    }
}