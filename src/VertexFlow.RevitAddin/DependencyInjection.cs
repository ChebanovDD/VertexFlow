using Autodesk.Revit.UI;
using Autofac;
using VertexFlow.RevitAddin.Exporter;
using VertexFlow.RevitAddin.Interfaces;
using VertexFlow.RevitAddin.Interfaces.Exporter;
using VertexFlow.RevitAddin.Interfaces.Services;
using VertexFlow.RevitAddin.RibbonPanels;
using VertexFlow.RevitAddin.Services;

namespace VertexFlow.RevitAddin
{
    public static class DependencyInjection
    {
        private const string Server = "https://localhost:5001";
        
        public static IContainer Configure(this UIControlledApplication application)
        {
            return application
                .AddRibbonPanel<VertexFlowRibbonPanel>()
                .BuildContainer();
        }
        
        private static IContainer BuildContainer(this UIControlledApplication application)
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<UpdaterService>().As<IUpdaterService>().SingleInstance();
            builder.RegisterType<GeometryService>().As<IGeometryService>().SingleInstance();
            builder.RegisterInstance<IGeometryExporter>(new GeometryExporter(Server)).SingleInstance();
            
            builder.RegisterInstance<UIControlledApplication>(application).SingleInstance();
            
            return builder.Build();
        }
        
        private static UIControlledApplication AddRibbonPanel<TRibbonPanel>(this UIControlledApplication application)
            where TRibbonPanel : IRibbonPanel, new()
        {
            new TRibbonPanel().Create(application);
            return application;
        }
    }
}