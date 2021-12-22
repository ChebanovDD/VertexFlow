using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VertexFlow.Contracts.Responses;
using VertexFlow.WebAPI.Interfaces;
using VertexFlow.WebApplication.Interfaces.Services;

namespace VertexFlow.WebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectDataMapper _mapper;
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService, IProjectDataMapper mapper)
        {
            _mapper = mapper;
            _projectService = projectService;
        }
        
        [HttpPost("{projectName}")]
        public async Task<IActionResult> Create(string projectName, CancellationToken cancellationToken)
        {
            await _projectService.CreateAsync(projectName, cancellationToken);
            return NoContent();
        }

        [HttpGet("{projectName}")]
        public async Task<IActionResult> Get(string projectName, CancellationToken cancellationToken)
        {
            var project = await _projectService.GetAsync(projectName, cancellationToken);
            return Ok(_mapper.ToResponse(project));
        }

        [HttpGet]
        public async IAsyncEnumerable<ProjectResponse> GetAll([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var project in _projectService.GetAllAsync(cancellationToken))
            {
                yield return _mapper.ToResponse(project);
            }
        }
        
        [HttpDelete("{projectName}")]
        public async Task<IActionResult> Delete(string projectName, CancellationToken cancellationToken)
        {
            await _projectService.DeleteAsync(projectName, cancellationToken);
            return NoContent();
        }
    }
}