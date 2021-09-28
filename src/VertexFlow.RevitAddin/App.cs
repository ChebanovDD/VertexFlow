using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Extensions;
using VertexFlow.RevitAddin.Interfaces;
using VertexFlow.RevitAddin.RibbonPanels;

namespace VertexFlow.RevitAddin
{
    public class App : IExternalApplication, IApp
    {
        public Result OnStartup(UIControlledApplication application)
        {
            application.AddRibbonPanel<VertexFlowRibbonPanel>();
            return Result.Succeeded;
        }
        
        public void ExportSelectedElements(IEnumerable<ElementId> elementIds)
        {
            TaskDialog.Show("VertexFlow", "ExportSelectedElements.");
        }
        
        public void SubscribeToElementsChanges(IEnumerable<ElementId> elementIds)
        {
            TaskDialog.Show("VertexFlow", "SubscribeToElementsChanges.");
        }
        
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}