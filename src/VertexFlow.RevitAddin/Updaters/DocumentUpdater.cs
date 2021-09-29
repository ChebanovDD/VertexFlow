using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using VertexFlow.RevitAddin.Interfaces;

namespace VertexFlow.RevitAddin.Updaters
{
    public class DocumentUpdater : IAppUpdater
    {
        private readonly UIControlledApplication _application;
        
        public event UpdaterEventHandler Modified;
        
        public DocumentUpdater(UIControlledApplication application)
        {
            _application = application;
            _application.ControlledApplication.DocumentChanged += OnDocumentChanged;
        }
        
        private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            if (e.Operation == UndoOperation.TransactionUndone)
            {
                Modified?.Invoke(e.GetDocument(), e.GetModifiedElementIds());
            }
        }
        
        public void Dispose()
        {
            _application.ControlledApplication.DocumentChanged -= OnDocumentChanged;
        }
    }
}