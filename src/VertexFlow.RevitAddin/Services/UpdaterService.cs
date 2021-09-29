using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Interfaces;
using VertexFlow.RevitAddin.Updaters;

namespace VertexFlow.RevitAddin.Services
{
    public class UpdaterService : IAppService
    {
        private ElementId[] _registeredElementIds;
        private readonly GeometryService _geometryService;
        private readonly List<IAppUpdater> _updaters = new List<IAppUpdater>();
        
        public UpdaterService(UIControlledApplication application, GeometryService geometryService)
        {
            _geometryService = geometryService;
            AddUpdater(new DocumentUpdater(application));
            AddUpdater(new GeometryUpdater(application.ActiveAddInId));
        }
        
        public void SubscribeToElementsChanges(ICollection<ElementId> elementIds)
        {
            _registeredElementIds = elementIds.ToArray();
        }
        
        public void Dispose()
        {
            for (var i = _updaters.Count - 1; i >= 0; i--)
            {
                RemoveUpdater(i);
            }
        }
        
        private void AddUpdater(IAppUpdater updater)
        {
            updater.Modified += OnModified;
            _updaters.Add(updater);
        }

        private void OnModified(Document document, IEnumerable<ElementId> elementIds)
        {
            if (HasRegisteredElements())
            {
                _geometryService.UpdateElements(document, GetRegisteredIds(elementIds));
            }
        }
        
        private bool HasRegisteredElements()
        {
            return _registeredElementIds != null && _registeredElementIds.Length != 0;
        }
        
        private IEnumerable<ElementId> GetRegisteredIds(IEnumerable<ElementId> elementIds)
        {
            return elementIds.Where(elementId => _registeredElementIds.Contains(elementId));
        }
        
        private void RemoveUpdater(int index)
        {
            _updaters[index].Modified -= OnModified;
            _updaters[index].Dispose();
            _updaters.RemoveAt(index);
        }
    }
}