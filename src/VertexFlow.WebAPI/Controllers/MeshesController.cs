using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VertexFlow.WebApplication.Interfaces.Services;

namespace VertexFlow.WebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]/{projectName}/{meshId}")]
    public class MeshesController : ControllerBase
    {
        private const string StreamContentType = "application/octet-stream";
        
        private readonly IMeshService _meshService;

        public MeshesController(IMeshService meshService)
        {
            _meshService = meshService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(string projectName, string meshId, [FromBody] Stream meshData,
            CancellationToken cancellationToken)
        {
            await using (meshData)
            {
                await _meshService.AddAsync(projectName, meshId, meshData, cancellationToken);
                return NoContent();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(string projectName, string meshId, CancellationToken cancellationToken)
        {
            var meshData = await _meshService.GetAsync(projectName, meshId, cancellationToken);
            return File(meshData, StreamContentType);
        }

        [HttpPut]
        public async Task<IActionResult> Update(string projectName, string meshId, [FromBody] Stream meshData,
            CancellationToken cancellationToken)
        {
            await using (meshData)
            {
                await _meshService.UpdateAsync(projectName, meshId, meshData, cancellationToken);
                return NoContent();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string projectName, string meshId, CancellationToken cancellationToken)
        {
            await _meshService.DeleteAsync(projectName, meshId, cancellationToken);
            return NoContent();
        }
    }
}