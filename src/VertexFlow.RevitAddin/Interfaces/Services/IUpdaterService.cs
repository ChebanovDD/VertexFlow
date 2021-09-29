using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Interfaces.Services
{
    public interface IUpdaterService : IDisposable
    {
        void SubscribeToElementsChanges(IEnumerable<ElementId> elementIds);
    }
}