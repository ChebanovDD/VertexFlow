using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Exporter
{
    public class MeshExtractor
    {
        private readonly Options _geometryOptions;

        public MeshExtractor()
        {
            _geometryOptions = new Options();
        }

        public List<Mesh> GetMeshes(Element element)
        {
            var meshes = new List<Mesh>();
            var geometryElement = element.get_Geometry(_geometryOptions);

            foreach (var geometry in geometryElement)
            {
                if (IsSolid(geometry, out var solid))
                {
                    meshes.AddRange(GetMeshes(solid));
                    continue;
                }

                if (IsMesh(geometry, out var mesh))
                {
                    meshes.Add(mesh);
                }
            }

            return meshes;
        }

        private bool IsSolid(GeometryObject geometry, out Solid solid)
        {
            solid = geometry as Solid;
            return solid != null && 0 < solid.Faces.Size;
        }

        private bool IsMesh(GeometryObject geometry, out Mesh mesh)
        {
            mesh = geometry as Mesh;
            return mesh != null && 0 < mesh.NumTriangles;
        }

        private Mesh[] GetMeshes(Solid solid)
        {
            var facesCount = solid.Faces.Size;
            var meshes = new Mesh[facesCount];

            for (var i = 0; i < facesCount; i++)
            {
                meshes[i] = solid.Faces.get_Item(i).Triangulate();
            }

            return meshes;
        }
    }
}