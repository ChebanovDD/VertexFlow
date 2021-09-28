using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Interfaces;

namespace VertexFlow.RevitAddin.Commands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class ExportGeometryCommand : ExternalCommand
    {
        public override string Cmd => "cmdExportGeometry";
        public override string Name => "Export Geometry";
        public override string ToolTip => "Export selected elements";
        
        protected override void OnExecute(IApp app, ExternalCommandData commandData)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            var elementIds = uiDoc.Selection.GetElementIds();
            
            app.ExportSelectedElements(elementIds);
        }
    }
}