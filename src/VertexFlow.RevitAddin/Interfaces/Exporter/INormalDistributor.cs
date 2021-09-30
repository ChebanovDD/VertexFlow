using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Interfaces.Exporter
{
    public interface INormalDistributor
    {
        DistributionOfNormals DistributionOfNormals { get; }
        
        void Configure(Mesh mesh, XYZ flipVector);
        XYZ GetNormal(int i);
        void Reset();
    }
}