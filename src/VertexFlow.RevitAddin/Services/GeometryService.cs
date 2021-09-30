using System.Collections.Generic;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using VertexFlow.RevitAddin.Extensions;
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
        
        public void ExportElement(Element element)
        {
            SendOrUpdateElement(element).Forget(false);
        }

        public void UpdateElement(Element element)
        {
            SendOrUpdateElement(element).Forget(false);
        }

        public void ExportElements(IEnumerable<Element> elements)
        {
            SendOrUpdateElements(elements).Forget(false);
        }
        
        public void UpdateElements(IEnumerable<Element> elements)
        {
            SendOrUpdateElements(elements).Forget(false);
        }
        
        public void Dispose()
        {
            _geometryExporter?.Dispose();
        }
        
        private async Task SendOrUpdateElement(Element element)
        {
            await SendOrUpdateElementAsync(element).ConfigureAwait(false);
        }
        
        private async Task SendOrUpdateElements(IEnumerable<Element> elements)
        {
            var tasks = new List<Task>();
            
            foreach (var element in elements)
            {
                tasks.Add(SendOrUpdateElementAsync(element));
            }
            
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        
        private async Task SendOrUpdateElementAsync(Element element)
        {
            await _geometryExporter.SendOrUpdateElementAsync(element).ConfigureAwait(false);
        }
    }
}