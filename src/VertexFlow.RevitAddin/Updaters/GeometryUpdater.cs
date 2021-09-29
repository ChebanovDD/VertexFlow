using System;
using Autodesk.Revit.DB;
using VertexFlow.RevitAddin.Interfaces;

namespace VertexFlow.RevitAddin.Updaters
{
    public class GeometryUpdater : IAppUpdater, IUpdater
    {
        private readonly UpdaterId _updaterId;
        
        public event UpdaterEventHandler Modified;
        
        public GeometryUpdater(AddInId addInId)
        {
            _updaterId = new UpdaterId(addInId, Guid.NewGuid());
            RegisterUpdater();
        }
        
        private void RegisterUpdater()
        {
            UpdaterRegistry.RegisterUpdater(this, true);
            UpdaterRegistry.AddTrigger(_updaterId, new ElementClassFilter(typeof(HostObject)),
                Element.GetChangeTypeGeometry());
            UpdaterRegistry.AddTrigger(_updaterId, new ElementClassFilter(typeof(FamilyInstance)),
                Element.GetChangeTypeGeometry());
        }
        
        public void Execute(UpdaterData data)
        {
            Modified?.Invoke(data.GetDocument(), data.GetModifiedElementIds());
        }
        
        private void UnregisterUpdater()
        {
            UpdaterRegistry.RemoveAllTriggers(GetUpdaterId());
            UpdaterRegistry.UnregisterUpdater(GetUpdaterId());
        }
        
        public UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }
        
        public ChangePriority GetChangePriority()
        {
            return ChangePriority.FreeStandingComponents;
        }
        
        public string GetUpdaterName()
        {
            return "NormsUpdater";
        }
        
        public string GetAdditionalInformation()
        {
            return "N/A";
        }
        
        public void Dispose()
        {
            UnregisterUpdater();
            _updaterId?.Dispose();
        }
    }
}