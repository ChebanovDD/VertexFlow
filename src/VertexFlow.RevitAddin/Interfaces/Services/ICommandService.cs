using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Interfaces.Services
{
    public interface ICommandService
    {
        void ExportElements(Document document, ICollection<ElementId> elementIds);
        void SubscribeToElementsChanges(ICollection<ElementId> elementIds);
    }
}