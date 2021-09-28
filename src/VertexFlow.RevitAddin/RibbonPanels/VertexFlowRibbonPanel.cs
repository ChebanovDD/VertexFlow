using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Commands;

namespace VertexFlow.RevitAddin.RibbonPanels
{
    public class VertexFlowRibbonPanel : ExternalRibbonPanel
    {
        protected override string PanelName => "Vertex Flow";
        
        protected override void OnBuild(RibbonPanel ribbonPanel)
        {
            var exportGeometryButton = CreatePushButton<ExportGeometryCommand>();
            var subscribeToChangesButton = CreatePushButton<SubscribeToChangesCommand>();
            
            ribbonPanel.AddStackedItems(exportGeometryButton, subscribeToChangesButton);
        }
    }
}