using System;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Interfaces.Exporter
{
    public interface IGeometryExporter : IDisposable
    {
        Task SendOrUpdateElementAsync(Element element);
    }
}