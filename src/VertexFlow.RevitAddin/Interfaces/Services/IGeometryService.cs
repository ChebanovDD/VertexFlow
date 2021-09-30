using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Interfaces.Services
{
    public interface IGeometryService : IDisposable
    {
        void ExportElement(Element element);
        void UpdateElement(Element element);
        void ExportElements(IEnumerable<Element> elements);
        void UpdateElements(IEnumerable<Element> elements);
    }
}