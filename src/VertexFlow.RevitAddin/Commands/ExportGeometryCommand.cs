using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;

namespace VertexFlow.RevitAddin.Commands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class ExportGeometryCommand : AppCommand<GeometryExporter>
    {
        public override string Cmd => "cmdExportGeometry";
        public override string Name => "Export Geometry";
        public override string ToolTip => "Export selected elements";
        
        protected override void OnExecute(GeometryExporter service, ExternalCommandData commandData)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            var elementIds = uiDoc.Selection.GetElementIds();
            
            service.ExportSelectedElements(uiDoc, elementIds);
        }
    }
}