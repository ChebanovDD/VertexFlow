using System;

namespace VertexFlow.SDK.Listeners
{
    /// <summary>
    /// Contains mesh event data.
    /// </summary>
    public class MeshEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MeshEventArgs"/>.
        /// </summary>
        /// <param name="projectName">Project name.</param>
        /// <param name="meshId">Mesh id.</param>
        public MeshEventArgs(string projectName, string meshId)
        {
            MeshId = meshId;
            ProjectName = projectName;
        }

        /// <summary>
        /// Mesh id.
        /// </summary>
        public string MeshId { get; }
        
        /// <summary>
        /// Project name.
        /// </summary>
        public string ProjectName { get; }
    }
}