using Autodesk.Revit.DB;
using VertexFlow.RevitAddin.Extensions;

namespace VertexFlow.RevitAddin.Exporter.NormalDistributors
{
    public class OnePerFaceNormalDistributor : NormalDistributor
    {
        private XYZ _normal;

        public override DistributionOfNormals DistributionOfNormals => DistributionOfNormals.OnePerFace;

        public override void Configure(Mesh mesh, XYZ flipVector)
        {
            if (IsValid(mesh))
            {
                _normal = mesh.GetNormal(0).Scale(flipVector);
            }
        }
        
        public override XYZ GetNormal(int i)
        {
            return _normal;
        }
        
        public override void Reset()
        {
            _normal = null;
        }
    }
}