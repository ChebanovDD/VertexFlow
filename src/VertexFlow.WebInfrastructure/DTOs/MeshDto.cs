using Newtonsoft.Json;
using VertexFlow.Core.Structs;

namespace VertexFlow.WebInfrastructure.DTOs
{
    internal record MeshDto
    {
        [JsonProperty(PropertyName = "id")]
        public string Guid { get; init; }
        
        [JsonProperty(PropertyName = "triangles")]
        public int[] Triangles { get; init; }
        
        [JsonProperty(PropertyName = "vertices")]
        public Vector3[] Vertices { get; init; }
        
        [JsonProperty(PropertyName = "normals")]
        public Vector3[] Normals { get; init; }
    }
}