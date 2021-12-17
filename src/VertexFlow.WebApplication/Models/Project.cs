using System.Collections.Generic;

namespace VertexFlow.WebApplication.Models
{
    public record Project(string Name, Dictionary<string, string> MeshIds);
}