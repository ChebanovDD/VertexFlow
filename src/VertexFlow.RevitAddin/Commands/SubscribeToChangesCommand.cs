using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Services;

namespace VertexFlow.RevitAddin.Commands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class SubscribeToChangesCommand : AppCommand<UpdaterService>
    {
        public override string Cmd => "cmdSubscribeToChanges";
        public override string Name => "Subscribe To Changes";
        public override string ToolTip => "Subscribe to changes selected elements";
        
        protected override void OnExecute(UpdaterService updaterService, ExternalCommandData commandData)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            var elementIds = uiDoc.Selection.GetElementIds();
            
            updaterService.SubscribeToElementsChanges(elementIds);
        }
    }
}