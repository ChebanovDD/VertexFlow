using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Interfaces;

namespace VertexFlow.RevitAddin.Commands
{
    public abstract class AppCommand<TService> : IAppCommand, IExternalCommand
    {
        public abstract string Cmd { get; }
        public abstract string Name { get; }
        public abstract string ToolTip { get; }
        
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var app = GetExternalApplication<App>(commandData);
            if (app == null)
            {
                message = $"The {typeof(App).FullName} not found.";
                return Result.Failed;
            }
            
            OnExecute(app.Resolve<TService>(), commandData);
            return Result.Succeeded;
        }
        
        protected abstract void OnExecute(TService service, ExternalCommandData commandData);
        
        private IApp GetExternalApplication<T>(ExternalCommandData commandData) where T : IApp
        {
            var externalApps = commandData.Application.LoadedApplications;
            
            for (var i = 0; i < externalApps.Size; i++)
            {
                if (externalApps.get_Item(i).GetType() == typeof(T))
                {
                    return (IApp)externalApps.get_Item(i);
                }
            }
            
            return default;
        }
    }
}