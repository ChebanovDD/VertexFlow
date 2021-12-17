using System.Collections.Generic;

namespace VertexFlow.Core.Models
{
    public class ProjectData
    {
        public string Name { get; set; }
        public IEnumerable<string> MeshIds { get; set; }
    }
}