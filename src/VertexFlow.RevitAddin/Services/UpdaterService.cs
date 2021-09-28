using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Interfaces;
using VertexFlow.RevitAddin.Updaters;

namespace VertexFlow.RevitAddin.Services
{
    public class UpdaterService : IAppService
    {
        private IUpdater _geometryUpdater;
        private readonly UIControlledApplication _application;
        
        public UpdaterService(UIControlledApplication application)
        {
            _application = application;
            _application.ControlledApplication.DocumentChanged += OnDocumentChanged;
            _application.ControlledApplication.ApplicationInitialized += OnApplicationInitialized;
        }
        
        private void OnApplicationInitialized(object sender, ApplicationInitializedEventArgs e)
        {
            _geometryUpdater = new GeometryUpdater(_application.ActiveAddInId);
            RegisterUpdater(_geometryUpdater);
        }
        
        private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            _application.ControlledApplication.DocumentChanged -= OnDocumentChanged;
            _application.ControlledApplication.ApplicationInitialized -= OnApplicationInitialized;
            UnregisterUpdater(_geometryUpdater);
        }
        
        private void RegisterUpdater(IUpdater updater)
        {
            UpdaterRegistry.RegisterUpdater(updater, true);
            AddTrigger(updater.GetUpdaterId(), typeof(HostObject));
            AddTrigger(updater.GetUpdaterId(), typeof(FamilyInstance));
        }
        
        private void AddTrigger(UpdaterId updaterId, Type type)
        {
            UpdaterRegistry.AddTrigger(updaterId, new ElementClassFilter(type), Element.GetChangeTypeGeometry());
        }
        
        private void UnregisterUpdater(IUpdater updater)
        {
            UpdaterRegistry.RemoveAllTriggers(updater.GetUpdaterId());
            UpdaterRegistry.UnregisterUpdater(updater.GetUpdaterId());
        }
    }
}