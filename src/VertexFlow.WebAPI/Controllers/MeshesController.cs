using System.Collections.Generic;
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
        public async Task<IActionResult> Create([FromBody] MeshRequest meshRequest)
        {
            await _meshService.AddAsync(_mapper.FromRequest(meshRequest));
            return CreatedAtAction(nameof(Get), new {meshId = meshRequest.Id}, meshRequest);
        }

        [HttpGet("{meshId}")]
        public async Task<IActionResult> Get(string meshId)
        {
            var mesh = await _meshService.GetAsync(meshId); 
            return Ok(_mapper.ToResponse(mesh));
        }
        
        [HttpGet]
        public async IAsyncEnumerable<MeshResponse> GetAll()
        {
            await foreach (var mesh in _meshService.GetAllAsync())
            {
                yield return _mapper.ToResponse(mesh);
            }
        }

        [HttpPut("{meshId}")]
        public async Task<IActionResult> Update(string meshId, [FromBody] MeshRequest meshRequest)
        {
            await _meshService.UpdateAsync(meshId, _mapper.FromRequest(meshRequest));
            return NoContent();
        }

        [HttpDelete("{meshId}")]
        public async Task<IActionResult> Delete(string meshId)
        {
            await _meshService.DeleteAsync(meshId);
            return NoContent();
        }
    }
}