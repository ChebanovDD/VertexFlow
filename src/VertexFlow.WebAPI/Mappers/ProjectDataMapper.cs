using VertexFlow.Contracts.Responses;
using VertexFlow.WebAPI.Interfaces;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebAPI.Mappers
{
    public class ProjectDataMapper : IProjectDataMapper
    {
        public ProjectResponse ToResponse(Project project)
        {
            return new ProjectResponse
            {
                Name = project.Name,
                MeshIds = project.MeshIds.Keys
            };
        }
    }
}