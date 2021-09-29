using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Interfaces.Services;

namespace VertexFlow.RevitAddin.Commands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class SubscribeToChangesCommand : AppCommand<IUpdaterService>
    {
        public override string Cmd => "cmdSubscribeToChanges";
        public override string Name => "Subscribe To Changes";
        public override string ToolTip => "Subscribe to changes selected elements";
        
        protected override void OnExecute(IUpdaterService updaterService, ExternalCommandData commandData)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            var elementIds = uiDoc.Selection.GetElementIds();
            
            updaterService.SubscribeToElementsChanges(elementIds);
        }
    }
}