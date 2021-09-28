using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Extensions;
using VertexFlow.RevitAddin.Interfaces;

namespace VertexFlow.RevitAddin.Commands
{
    public abstract class ExternalCommand : IExternalCommand
    {
        public abstract string Cmd { get; }
        public abstract string Name { get; }
        public abstract string ToolTip { get; }
        
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var app = commandData.GetExternalApplication<App>();
            if (app == null)
            {
                message = $"The {typeof(App).FullName} not found.";
                return Result.Failed;
            }

            OnExecute(app, commandData);
            return Result.Succeeded;
        }
        
        protected abstract void OnExecute(IApp app, ExternalCommandData commandData);
    }
}