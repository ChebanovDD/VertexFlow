using Autodesk.Revit.DB;

namespace VertexFlow.RevitAddin.Exporter.NormalDistributors
{
    public class OnEachFacetNormalDistributor : NormalDistributor
    {
        public override DistributionOfNormals DistributionOfNormals => DistributionOfNormals.OnEachFacet;
        
        public override void Configure(Mesh mesh, XYZ flipVector)
        {
        }
        
        public override XYZ GetNormal(int i)
        {
            throw new System.NotImplementedException();
        }
        
        public override void Reset()
        {
        }
    }
}