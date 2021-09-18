using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VertexFlow.WebAPI.Contracts;
using VertexFlow.WebApplication.Interfaces.Services;

namespace VertexFlow.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeshController : ControllerBase
    {
        private readonly IMeshService _meshService;

        public MeshController(IMeshService meshService)
        {
            _meshService = meshService;
        }
        
        [HttpPost]
        public Task<IActionResult> Create([FromBody] MeshRequest meshRequest)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet("{meshId}")]
        public Task<IActionResult> Get(string meshId)
        {
            throw new System.NotImplementedException();
        }
        
        [HttpGet]
        public IAsyncEnumerable<MeshResponse> GetAll()
        {
            throw new System.NotImplementedException();
        }

        [HttpPut("{meshId}")]
        public Task<IActionResult> Update([FromBody] MeshRequest meshRequest)
        {
            throw new System.NotImplementedException();
        }

        [HttpDelete("{meshId}")]
        public Task<IActionResult> Delete(string meshId)
        {
            throw new System.NotImplementedException();
        }
    }
}