using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Exporter.Models
{
    public class RevitMesh
    {
        public string Id { get; }
        public List<XYZ> Normals { get; }
        public List<XYZ> Vertices { get; }
        public List<int> Triangles { get; }

        public RevitMesh(string id)
        {
            Id = id;
            Normals = new List<XYZ>();
            Vertices = new List<XYZ>();
            Triangles = new List<int>();
        }
    }
}