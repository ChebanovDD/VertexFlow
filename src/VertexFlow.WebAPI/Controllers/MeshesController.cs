using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VertexFlow.Contracts.Requests;
using VertexFlow.Contracts.Responses;
using VertexFlow.WebAPI.Interfaces;
using VertexFlow.WebApplication.Interfaces.Services;

namespace VertexFlow.WebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]/{projectName}")]
    public class MeshesController : ControllerBase
    {
        private readonly IMeshDataMapper _mapper;
        private readonly IMeshService _meshService;

        public MeshesController(IMeshService meshService, IMeshDataMapper mapper)
        {
            _mapper = mapper;
            _meshService = meshService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(string projectName, [FromBody] MeshRequest meshRequest,
            CancellationToken cancellationToken)
        {
            await _meshService.AddAsync(projectName, _mapper.FromRequest(meshRequest), cancellationToken);
            return NoContent();
        }

        [HttpGet("{meshId}")]
        public async Task<IActionResult> Get(string projectName, string meshId, CancellationToken cancellationToken)
        {
            var mesh = await _meshService.GetAsync(projectName, meshId, cancellationToken);
            return Ok(_mapper.ToResponse(mesh));
        }

        [HttpGet]
        public async IAsyncEnumerable<MeshResponse> GetAll(string projectName,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var mesh in _meshService.GetAllAsync(projectName, cancellationToken))
            {
                yield return _mapper.ToResponse(mesh);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(string projectName, [FromBody] MeshRequest meshRequest,
            CancellationToken cancellationToken)
        {
            await _meshService.UpdateAsync(projectName, _mapper.FromRequest(meshRequest), cancellationToken);
            return NoContent();
        }

        [HttpDelete("{meshId}")]
        public async Task<IActionResult> Delete(string projectName, string meshId, CancellationToken cancellationToken)
        {
            await _meshService.DeleteAsync(projectName, meshId, cancellationToken);
            return NoContent();
        }
    }
}