﻿using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Interfaces
{
    public delegate void UpdaterEventHandler(Document document, ICollection<ElementId> elementIds);
    
    public interface IAppUpdater : IDisposable
    {
        event UpdaterEventHandler Modified;
    }
}