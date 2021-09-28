using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Interfaces
{
    public interface IApp
    {
        void ExportSelectedElements(IEnumerable<ElementId> elementIds);
        void SubscribeToElementsChanges(IEnumerable<ElementId> elementIds);
    }
}