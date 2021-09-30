using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Interfaces.Services
{
    public interface IGeometryService : IDisposable
    {
        void ExportElements(Document document, IEnumerable<ElementId> elementIds);
        void UpdateElements(Document document, IEnumerable<ElementId> elementIds);
    }
}