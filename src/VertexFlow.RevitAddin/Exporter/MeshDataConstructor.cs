using System.Collections.Generic;
using Autodesk.Revit.DB;
using VertexFlow.RevitAddin.Exporter.Models;
using VertexFlow.RevitAddin.Exporter.NormalDistributors;
using VertexFlow.RevitAddin.Extensions;
using VertexFlow.RevitAddin.Interfaces.Exporter;

namespace VertexFlow.RevitAddin.Exporter
{
    public class MeshDataConstructor
    {
        private readonly XYZ _flipX;
        private readonly Dictionary<DistributionOfNormals, INormalDistributor> _normalDistributors;

        public MeshDataConstructor()
        {
            _flipX = new XYZ(-1, 1, 1);
            
            _normalDistributors = new Dictionary<DistributionOfNormals, INormalDistributor>
            {
                { DistributionOfNormals.AtEachPoint, new AtEachPointNormalDistributor() },
                { DistributionOfNormals.OnEachFacet, new OnEachFacetNormalDistributor() },
                { DistributionOfNormals.OnePerFace, new OnePerFaceNormalDistributor() }
            };
        }
        
        public RevitMesh ConstructMeshData(List<Mesh> meshes, ElementId elementId)
        {
            var triangleIndexOffset = 0;
            var meshData = new RevitMesh($"{elementId}");

            for (var i = 0; i < meshes.Count; i++)
            {
                var mesh = meshes[i];
                
                AddVertices(meshData, mesh);
                AddTriangles(meshData, mesh, triangleIndexOffset);
                
                triangleIndexOffset += mesh.Vertices.Count;
            }
            
            return meshData;
        }

        private void AddVertices(RevitMesh meshData, Mesh mesh)
        {
            var normalDistributor = _normalDistributors[mesh.DistributionOfNormals];
            normalDistributor.Configure(mesh, _flipX);
            
            var verticesCount = mesh.Vertices.Count;
            for (var i = 0; i < verticesCount; i++)
            {
                meshData.Vertices.Add(mesh.Vertices[i].Scale(_flipX));
                
                if (normalDistributor.DistributionOfNormals != DistributionOfNormals.OnEachFacet)
                {
                    meshData.Normals.Add(normalDistributor.GetNormal(i));
                }
            }
            
            normalDistributor.Reset();
        }

        private void AddTriangles(RevitMesh meshData, Mesh mesh, int triangleIndexOffset)
        {
            var trianglesCount = mesh.NumTriangles;
            for (var i = 0; i < trianglesCount; i++)
            {
                var triangle = mesh.get_Triangle(i);
                
                meshData.Triangles.Add((int)triangle.get_Index(2) + triangleIndexOffset);
                meshData.Triangles.Add((int)triangle.get_Index(1) + triangleIndexOffset);
                meshData.Triangles.Add((int)triangle.get_Index(0) + triangleIndexOffset);
            }
        }
    }
}