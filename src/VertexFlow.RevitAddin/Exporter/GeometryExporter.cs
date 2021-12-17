using System.Threading.Tasks;
using Autodesk.Revit.DB;
using VertexFlow.RevitAddin.Exporter.Models;
using VertexFlow.RevitAddin.Interfaces.Exporter;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.RevitAddin.Exporter
{
    public class GeometryExporter : IGeometryExporter
    {
        private readonly SDK.VertexFlow _vertexFlow;
        private readonly IMeshFlow<RevitMesh> _meshFlow;
        
        private readonly MeshExtractor _meshExtractor;
        private readonly MeshDataConstructor _meshDataConstructor;
        
        public GeometryExporter(string server)
        {
            _meshExtractor = new MeshExtractor();
            _meshDataConstructor = new MeshDataConstructor();
            
            _vertexFlow = new SDK.VertexFlow(server);
            _meshFlow = _vertexFlow.CreateMeshFlow<RevitMesh>("TestProject");
        }

        public void SetProject(string projectName)
        {
            _meshFlow.ProjectName = projectName;
        }

        public async Task SendOrUpdateElementAsync(Element element)
        {
            var meshes = _meshExtractor.GetMeshes(element);
            if (meshes.Count == 0)
            {
                return;
            }
            
            var meshData = _meshDataConstructor.ConstructMeshData(meshes, element.Id);
            await _meshFlow.UpdateAsync(meshData).ConfigureAwait(false);
        }
        
        public void Dispose()
        {
            _vertexFlow.Dispose();
        }
    }
}