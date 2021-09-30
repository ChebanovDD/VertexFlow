using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Extensions;
using VertexFlow.RevitAddin.Interfaces;
using VertexFlow.RevitAddin.Interfaces.Services;
using VertexFlow.RevitAddin.Updaters;

namespace VertexFlow.RevitAddin.Services
{
    public class UpdaterService : IUpdaterService
    {
        private ICollection<ElementId> _registeredElementIds;
        private readonly IGeometryService _geometryService;
        private readonly List<IAppUpdater> _updaters = new List<IAppUpdater>();
        
        public UpdaterService(UIControlledApplication application, IGeometryService geometryService)
        {
            _geometryService = geometryService;
            AddUpdater(new DocumentUpdater(application));
            AddUpdater(new GeometryUpdater(application.ActiveAddInId));
        }
        
        public void SubscribeToElementsChanges(ICollection<ElementId> elementIds)
        {
            _registeredElementIds = elementIds;
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

        private void OnModified(Document document, ICollection<ElementId> elementIds)
        {
            if (_registeredElementIds == null || _registeredElementIds.Count == 0)
            {
                return;
            }
            
            var elements = document.GetIntersectElements(elementIds, _registeredElementIds).ToList();
            if (elements.Count == 0)
            {
                return;
            }
            
            if (elements.Count == 1)
            {
                _geometryService.UpdateElement(elements[0]);
            }
            else
            {
                _geometryService.UpdateElements(elements);
            }
        }
        
        private void RemoveUpdater(int index)
        {
            _updaters[index].Modified -= OnModified;
            _updaters[index].Dispose();
            _updaters.RemoveAt(index);
        }
    }
}