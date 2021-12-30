using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VertexFlow.WebApplication.Interfaces;
using VertexFlow.WebApplication.Interfaces.Repositories;
using VertexFlow.WebInfrastructure.Repositories;

namespace VertexFlow.WebInfrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalR();
            services.AddSingleton<IMeshNotifier, MeshNotifier>();
            services.AddRepositoriesAsync(configuration).GetAwaiter().GetResult();
        }
        
        public static void MapNotifiers(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<MeshHub>("/notification");
        }

        private static async Task AddRepositoriesAsync(this IServiceCollection services, IConfiguration configuration)
        {
            var cosmosConfig = configuration.GetSection("CosmosDb");

            var database = await GetOrCreateDatabaseAsync(cosmosConfig).ConfigureAwait(false);
            var meshContainer = await CreateMeshRepositoryAsync(database, cosmosConfig).ConfigureAwait(false);
            var projectContainer = await CreateProjectRepositoryAsync(database, cosmosConfig).ConfigureAwait(false);

            var projectRepository = new ProjectRepository(projectContainer);
            var blobServiceClient =
                new BlobServiceClient(configuration.GetValue<string>("AzureBlobStorageConnectionString"));
            services.AddSingleton<IProjectRepository>(projectRepository);
            services.AddSingleton<IMeshRepository>(new MeshRepository(meshContainer, projectRepository, blobServiceClient));
        }

        private static async Task<Database> GetOrCreateDatabaseAsync(IConfiguration cosmosConfig)
        {
            var uri = cosmosConfig["Uri"];
            var key = cosmosConfig["Key"];
            var databaseName = cosmosConfig["DatabaseName"];

            var client = new CosmosClient(uri, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName).ConfigureAwait(false);

            return database.Database;
        }

        private static async Task<Container> CreateMeshRepositoryAsync(Database database, IConfiguration cosmosConfig)
        {
            return await CreateContainerAsync(database, cosmosConfig["MeshesContainerName"]);
        }

        private static async Task<Container> CreateProjectRepositoryAsync(Database database, IConfiguration cosmosConfig)
        {
            return await CreateContainerAsync(database, cosmosConfig["ProjectsContainerName"]);
        }

        private static async Task<Container> CreateContainerAsync(Database database, string containerName)
        {
            var response = await database
                .CreateContainerIfNotExistsAsync(containerName, "/id")
                .ConfigureAwait(false);

            return response.Container;
        }
    }
}