using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;

namespace VertexFlow.RevitAddin.Commands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class SubscribeToChangesCommand : AppCommand<GeometryExporter>
    {
        public override string Cmd => "cmdSubscribeToChanges";
        public override string Name => "Subscribe To Changes";
        public override string ToolTip => "Subscribe to changes selected elements";
        
        protected override void OnExecute(GeometryExporter service, ExternalCommandData commandData)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            var elementIds = uiDoc.Selection.GetElementIds();
            
            service.SubscribeToElementsChanges(uiDoc, elementIds);
        }
    }
}