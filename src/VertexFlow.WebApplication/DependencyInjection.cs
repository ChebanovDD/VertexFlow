using Microsoft.Extensions.DependencyInjection;
using VertexFlow.WebApplication.Interfaces.Services;
using VertexFlow.WebApplication.Services;

namespace VertexFlow.WebApplication
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IMeshService, MeshService>();
            services.AddSingleton<IProjectService, ProjectService>();
        }
    }
}