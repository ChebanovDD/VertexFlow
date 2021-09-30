using System.Collections.Generic;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using VertexFlow.RevitAddin.Extensions;
using VertexFlow.RevitAddin.Interfaces;
using VertexFlow.RevitAddin.Interfaces.Exporter;
using VertexFlow.RevitAddin.Interfaces.Services;

namespace VertexFlow.RevitAddin.Services
{
    public class GeometryService : IGeometryService
    {
        private readonly IGeometryExporter _geometryExporter;

        public GeometryService(IGeometryExporter geometryExporter)
        {
            _geometryExporter = geometryExporter;
        }
        
        public void ExportElements(Document document, IEnumerable<ElementId> elementIds)
        {
            SendOrUpdateElements(document, elementIds).Forget(false);
        }
        
        public void UpdateElements(Document document, IEnumerable<ElementId> elementIds)
        {
            SendOrUpdateElements(document, elementIds).Forget(false);
        }
        
        public void Dispose()
        {
            _geometryExporter?.Dispose();
        }
        
        private async Task SendOrUpdateElements(Document document, IEnumerable<ElementId> elementIds)
        {
            var tasks = new List<Task>();
            
            foreach (var elementId in elementIds)
            {
                tasks.Add(SendOrUpdateElementAsync(document.GetElement(elementId)));
            }
            
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        
        private async Task SendOrUpdateElementAsync(Element element)
        {
            await _geometryExporter.SendOrUpdateElementAsync(element).ConfigureAwait(false);
        }
    }
}