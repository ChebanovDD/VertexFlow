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
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Create([FromBody] MeshRequest meshRequest, CancellationToken cancellationToken)
        {
            await _meshService.AddAsync(_mapper.FromRequest(meshRequest), cancellationToken);
            return CreatedAtAction(nameof(Get), new {meshId = meshRequest.Id}, meshRequest);
        }

        [HttpGet("{meshId}")]
        public async Task<IActionResult> Get(string meshId, CancellationToken cancellationToken)
        {
            var mesh = await _meshService.GetAsync(meshId, cancellationToken); 
            return Ok(_mapper.ToResponse(mesh));
        }
        
        [HttpGet]
        public async IAsyncEnumerable<MeshResponse> GetAll([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var mesh in _meshService.GetAllAsync(cancellationToken))
            {
                yield return _mapper.ToResponse(mesh);
            }
        }

        [HttpPut("{meshId}")]
        public async Task<IActionResult> Update(string meshId, [FromBody] MeshRequest meshRequest, CancellationToken cancellationToken)
        {
            await _meshService.UpdateAsync(meshId, _mapper.FromRequest(meshRequest), cancellationToken);
            return NoContent();
        }

        [HttpDelete("{meshId}")]
        public async Task<IActionResult> Delete(string meshId, CancellationToken cancellationToken)
        {
            await _meshService.DeleteAsync(meshId, cancellationToken);
            return NoContent();
        }
    }
}