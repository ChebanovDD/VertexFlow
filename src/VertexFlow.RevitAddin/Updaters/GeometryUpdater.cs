using System;
using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Updaters
{
    public class GeometryUpdater : IUpdater
    {
        private readonly UpdaterId _updaterId;

        // public event EventHandler<IEnumerable<ElementId>> Modified;

        public GeometryUpdater(AddInId addInId)
        {
            _updaterId = new UpdaterId(addInId, Guid.NewGuid());
        }

        public void Execute(UpdaterData data)
        {
            // Modified?.Invoke(this, data.GetModifiedElementIds());
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
    }
}