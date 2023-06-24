using System.Net;
using BimDataControlPanel.BLL.Dtos.Change;
using BimDataControlPanel.BLL.Services;
using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.DAL.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BimDataControlPanel.WEB.Controllers.API
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ChangeApiController : Controller
    {
        private readonly ChangeService _changeService;

        public ChangeApiController(ChangeService changeService)
        {
            _changeService = changeService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChangeDto>>> GetAll()
        {
            var changes = await _changeService.GetAll();
            var dtos = changes.Select(p => new ChangeDto
            {
                Id = p.Id,
                Description = p.Description,
                ChangeTime = p.ChangeTime,
                ChangeType = p.ChangeType,
                UserRevitName = p.UserRevitName,
                ProjectId = p.ProjectId,
            });
            
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChangeDto>> Get(string? id)
        {
            var change = await _changeService.GetById(id);
            var dto = new ChangeDto
            {
                Id = change.Id,
                Description = change.Description,
                ChangeTime = change.ChangeTime,
                ChangeType = change.ChangeType,
                UserRevitName = change.UserRevitName,
                ProjectId = change.ProjectId,
            };

            return Ok(dto);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateChangeDto model)
        {
            await _changeService.Create(model);
            
            return Ok($"Change [{model.Description}] created!");
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string? id )
        {
            ModelState.Clear();
            await _changeService.Delete(id);
            
            return Ok($"Change [{id}] deleted!");
        }
    }
}