using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Commands;
using VertexFlow.RevitAddin.Interfaces;

namespace VertexFlow.RevitAddin.RibbonPanels
{
    public abstract class ExternalRibbonPanel : IRibbonPanel
    {
        protected abstract string PanelName { get; }
        
        public void Create(UIControlledApplication application)
        {
            OnBuild(application.CreateRibbonPanel(PanelName));
        }
        
        protected abstract void OnBuild(RibbonPanel ribbonPanel);
        
        protected ButtonData CreatePushButton<TCommand>() where TCommand : ExternalCommand, new()
        {
            var command = new TCommand();
            var commandType = typeof(TCommand);

            return new PushButtonData(command.Cmd, command.Name, commandType.Assembly.Location, commandType.FullName)
            {
                ToolTip = command.ToolTip
            };
        }
    }
}