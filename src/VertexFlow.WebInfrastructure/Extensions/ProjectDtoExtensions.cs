using VertexFlow.WebApplication.Models;
using VertexFlow.WebInfrastructure.DTOs;

namespace VertexFlow.WebInfrastructure.Extensions
{
    internal static class ProjectDtoExtensions
    {
        public static Project ToProject(this ProjectDto projectData)
        {
            return new Project
            (
                projectData.Name,
                projectData.MeshIds
            );
        }
    }
}