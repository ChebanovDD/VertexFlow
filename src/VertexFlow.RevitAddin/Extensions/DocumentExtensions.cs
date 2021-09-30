using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Extensions
{
    public static class DocumentExtensions
    {
        public static IEnumerable<Element> GetElements(this Document document, IEnumerable<ElementId> elementIds)
        {
            return elementIds.Select(document.GetElement);
        }

        public static IEnumerable<Element> GetIntersectElements(this Document document, IEnumerable<ElementId> first,
            IEnumerable<ElementId> second)
        {
            return first.Intersect(second).Select(document.GetElement);
        }
    }
}