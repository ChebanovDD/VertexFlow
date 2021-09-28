using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Interfaces;

namespace VertexFlow.RevitAddin.Extensions
{
    public static class AppExtensions
    {
        public static void AddRibbonPanel<TRibbonPanel>(this UIControlledApplication app)
            where TRibbonPanel : IRibbonPanel, new()
        {
            new TRibbonPanel().Create(app);
        }
        
        public static T GetExternalApplication<T>(this ExternalCommandData commandData) where T : IApp
        {
            var apps = commandData.Application.LoadedApplications;

            for (var i = 0; i < apps.Size; i++)
            {
                if (apps.get_Item(i).GetType() == typeof(T))
                {
                    return (T)apps.get_Item(i);
                }
            }

            return default;
        }
    }
}