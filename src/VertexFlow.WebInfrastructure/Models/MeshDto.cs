using Newtonsoft.Json;
using VertexFlow.Core.Structs;

namespace VertexFlow.WebInfrastructure.Models
{
    internal class MeshDto
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "triangles")]
        public int[] Triangles { get; set; }
        
        [JsonProperty(PropertyName = "vertices")]
        public Vector3[] Vertices { get; set; }
        
        [JsonProperty(PropertyName = "normals")]
        public Vector3[] Normals { get; set; }
    }
}