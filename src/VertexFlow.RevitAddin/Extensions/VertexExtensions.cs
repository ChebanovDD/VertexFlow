using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Extensions
{
    public static class VertexExtensions
    {
        public static XYZ Scale(this XYZ v1, XYZ v2)
        {
            return new XYZ(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }
    }
}