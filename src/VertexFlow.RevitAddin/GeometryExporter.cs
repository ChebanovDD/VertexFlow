using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Interfaces;

namespace VertexFlow.RevitAddin
{
    public class GeometryExporter : IAppService
    {
        public void ExportSelectedElements(UIDocument uiDoc, IEnumerable<ElementId> elementIds)
        {
            TaskDialog.Show("VertexFlow", "ExportSelectedElements.");
        }

        public void SubscribeToElementsChanges(UIDocument uiDoc, IEnumerable<ElementId> elementIds)
        {
            TaskDialog.Show("VertexFlow", "SubscribeToElementsChanges.");
        }

        public void Dispose()
        {
        }
    }
}