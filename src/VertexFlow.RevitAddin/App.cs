using Autodesk.Revit.UI;

namespace VertexFlow.RevitAddin
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            TaskDialog.Show("VertexFlow", $"OnStartup.");
            return Result.Succeeded;
        }
        
        public Result OnShutdown(UIControlledApplication application)
        {
            TaskDialog.Show("VertexFlow", $"OnShutdown.");
            return Result.Succeeded;
        }
    }
}