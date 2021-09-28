using Autodesk.Revit.UI;
using Autofac;
using VertexFlow.RevitAddin.Interfaces;

namespace VertexFlow.RevitAddin
{
    public class App : IExternalApplication, IApp
    {
        private ILifetimeScope _scope;
        
        public Result OnStartup(UIControlledApplication application)
        {
            _scope = application.Configure().BeginLifetimeScope();
            return Result.Succeeded;
        }
        
        public T Resolve<T>()
        {
            return _scope.Resolve<T>();
        }
        
        public Result OnShutdown(UIControlledApplication application)
        {
            _scope?.Dispose();
            return Result.Succeeded;
        }
    }
}