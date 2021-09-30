using System;
using Autodesk.Revit.DB;
using VertexFlow.RevitAddin.Interfaces.Exporter;

namespace VertexFlow.RevitAddin.Exporter.NormalDistributors
{
    public abstract class NormalDistributor : INormalDistributor
    {
        public abstract DistributionOfNormals DistributionOfNormals { get; }
        
        public abstract void Configure(Mesh mesh, XYZ flipVector);
        public abstract XYZ GetNormal(int i);
        public abstract void Reset();

        protected bool IsValid(Mesh mesh)
        {
            return mesh.DistributionOfNormals == DistributionOfNormals
                ? true
                : throw new ArgumentException(
                    $"Expected '{DistributionOfNormals}' but '{mesh.DistributionOfNormals}'", nameof(mesh));
        }
    }
}