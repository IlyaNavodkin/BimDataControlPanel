using System.Net;
using BimDataControlPanel.BLL.Dtos.Project;
using BimDataControlPanel.BLL.Services;
using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.DAL.Extensions;
using BimDataControlPanel.WEB.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BimDataControlPanel.WEB.Controllers.API
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProjectApiController : Controller
    {
        private readonly ProjectService _projectService;

        public ProjectApiController(ProjectService projectService)
        {
            _projectService = projectService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
        {
            var projects = await _projectService.GetAll();
            var dtos = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                RevitVersion = p.RevitVersion,
                Name = p.Name,
                CreationTime = p.CreationTime, 
                Complete = p.Complete
            });
            
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> Get(string? id)
        {
            var dto = await _projectService.GetDtoById(id);
            
            return Ok(dto);
        }

        [HttpPut]
        public async Task<ActionResult> Update(EditProjectDto dto)
        {
            await _projectService.Edit(dto);

            return Ok($"Project [{dto.Id}] updated!");
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto model)
        {
            await _projectService.Create(model);
            
            return Ok($"Project [{model.Name}] created!");
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string? id )
        {
            await _projectService.Delete(id);
            
            return Ok($"Project [{id}] deleted!");
        }
    }
}