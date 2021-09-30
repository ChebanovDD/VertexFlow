using Autodesk.Revit.DB;
using VertexFlow.RevitAddin.Extensions;

namespace VertexFlow.RevitAddin.Exporter.NormalDistributors
{
    public class AtEachPointNormalDistributor : NormalDistributor
    {
        private Mesh _mesh;
        private XYZ _flipVector;

        public override DistributionOfNormals DistributionOfNormals => DistributionOfNormals.AtEachPoint;
        
        public override void Configure(Mesh mesh, XYZ flipVector)
        {
            if (IsValid(mesh))
            {
                _mesh = mesh;
                _flipVector = flipVector;
            }
        }
        
        public override XYZ GetNormal(int i)
        {
            return _mesh.GetNormal(i).Scale(_flipVector);
        }
        
        public override void Reset()
        {
            _mesh = null;
            _flipVector = null;
        }
    }
}