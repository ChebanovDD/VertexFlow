using VertexFlow.Contracts.Responses;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebAPI.Interfaces
{
    public interface IProjectDataMapper
    {
        ProjectResponse ToResponse(Project project);
    }
}