using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Interfaces.Services;

namespace VertexFlow.RevitAddin.Commands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class ExportGeometryCommand : AppCommand<ICommandService>
    {
        public override string Cmd => "cmdExportGeometry";
        public override string Name => "Export Geometry";
        public override string ToolTip => "Export selected elements";
        
        protected override void OnExecute(ICommandService commandService, ExternalCommandData commandData)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            var elementIds = uiDoc.Selection.GetElementIds();
            
            commandService.ExportElements(uiDoc.Document, elementIds);
        }
    }
}