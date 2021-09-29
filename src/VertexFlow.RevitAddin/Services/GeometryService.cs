using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Interfaces;

namespace VertexFlow.RevitAddin.Services
{
    public class GeometryService : IAppService
    {
        public void ExportSelectedElements(Document document, IEnumerable<ElementId> elementIds)
        {
            TaskDialog.Show("VertexFlow", $"Export {elementIds.Count()} elements.");
        }
        
        public void UpdateElements(Document document, IEnumerable<ElementId> elementIds)
        {
            TaskDialog.Show("VertexFlow", $"Update {elementIds.Count()} elements.");
        }
        
        public void Dispose()
        {
        }
    }
}