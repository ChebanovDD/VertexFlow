using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Interfaces.Services
{
    public interface IGeometryService
    {
        void ExportSelectedElements(Document document, IEnumerable<ElementId> elementIds);
        void UpdateElements(Document document, IEnumerable<ElementId> elementIds);
    }
}