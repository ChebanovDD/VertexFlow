using System.Collections.Generic;
using Newtonsoft.Json;

namespace VertexFlow.WebInfrastructure.DTOs
{
    internal record ProjectDto
    {
        [JsonProperty(PropertyName = "id")]
        public string Name { get; init; }
        
        [JsonProperty(PropertyName = "meshIds")]
        public Dictionary<string, string> MeshIds { get; init; }
    }
}