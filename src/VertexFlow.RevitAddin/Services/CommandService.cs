using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using VertexFlow.RevitAddin.Extensions;
using VertexFlow.RevitAddin.Interfaces.Services;

namespace VertexFlow.RevitAddin.Services
{
    public class CommandService : ICommandService
    {
        private readonly IUpdaterService _updaterService;
        private readonly IGeometryService _geometryService;

        public CommandService(IUpdaterService updaterService, IGeometryService geometryService)
        {
            _updaterService = updaterService;
            _geometryService = geometryService;
        }
        
        public void ExportElements(Document document, ICollection<ElementId> elementIds)
        {
            if (elementIds.Count == 0)
            {
                return;
            }

            if (elementIds.Count == 1)
            {
                _geometryService.ExportElement(document.GetElement(elementIds.First()));
            }
            else
            {
                _geometryService.ExportElements(document.GetElements(elementIds));
            }
        }

        public void SubscribeToElementsChanges(ICollection<ElementId> elementIds)
        {
            _updaterService.SubscribeToElementsChanges(elementIds);
        }
    }
}