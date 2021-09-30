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
            RegisterUpdater(_updaterId);
        }
        
        private void RegisterUpdater(UpdaterId updaterId)
        {
            UpdaterRegistry.RegisterUpdater(this, true);
            UpdaterRegistry.AddTrigger(updaterId, new ElementClassFilter(typeof(HostObject)),
                Element.GetChangeTypeGeometry());
            UpdaterRegistry.AddTrigger(updaterId, new ElementClassFilter(typeof(FamilyInstance)),
                Element.GetChangeTypeGeometry());
        }
        
        public void Execute(UpdaterData data)
        {
            Modified?.Invoke(data.GetDocument(), data.GetModifiedElementIds());
        }
        
        private void UnregisterUpdater(UpdaterId updaterId)
        {
            UpdaterRegistry.RemoveAllTriggers(updaterId);
            UpdaterRegistry.UnregisterUpdater(updaterId);
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
            return "GeometryUpdater";
        }
        
        public string GetAdditionalInformation()
        {
            return "N/A";
        }
        
        public void Dispose()
        {
            UnregisterUpdater(_updaterId);
            _updaterId?.Dispose();
        }
    }
}