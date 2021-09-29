using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Services;

namespace VertexFlow.RevitAddin.Commands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class ExportGeometryCommand : AppCommand<GeometryService>
    {
        public override string Cmd => "cmdExportGeometry";
        public override string Name => "Export Geometry";
        public override string ToolTip => "Export selected elements";
        
        protected override void OnExecute(GeometryService geometryService, ExternalCommandData commandData)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            var elementIds = uiDoc.Selection.GetElementIds();
            
            geometryService.ExportSelectedElements(uiDoc.Document, elementIds);
        }
    }
}