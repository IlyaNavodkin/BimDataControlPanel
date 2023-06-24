using BimDataControlPanel.BLL.Dtos.Project;
using BimDataControlPanel.BLL.Services;
using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.DAL.Exeptions;
using BimDataControlPanel.WEB.Constants;
using BimDataControlPanel.WEB.Extensions;
using BimDataControlPanel.WEB.ViewModels.Pagination;
using BimDataControlPanel.WEB.ViewModels.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BimDataControlPanel.WEB.Controllers.MVC
{
    [Authorize(Roles = RolesConstants.AdminRoleName)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string searchString = "")
        {
            var projects = await _projectService.GetAll();
            var orderedProject = projects.OrderByDescending(p => p.CreationTime).ToList();

            orderedProject = SearchBy(searchString, orderedProject);

            var pageSize = 2; 
            var phonesPerPages=orderedProject.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var pageInfo = new PageInfo { PageNumber=page, PageSize=pageSize, TotalItems=orderedProject.Count};
            
            var viewModel = new ProjectListViewModel
            {
                Projects = phonesPerPages,
                PageInfo = pageInfo,
                SearchString = searchString
            };
            
            return View(viewModel);
        }

        private static List<Project> SearchBy(string searchString, List<Project> orderedProject)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                orderedProject = orderedProject.Where(c => c.Name.Contains(searchString)).ToList();
            }

            return orderedProject;
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectDto model)
        {
            ModelState.Clear();
            await _projectService.Create(model);

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string? id)
        {
            ModelState.Clear();
            await _projectService.Delete(id);
        
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            var dto = await _projectService.GetEditDto(id);    
            
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectDto dto)
        {
            ModelState.Clear();

            try
            {
                await _projectService.Edit(dto);
                return RedirectToAction("Index");
            }
            catch (ValidationException exception)
            {
                await this.GenerateModelStateErrors(exception);
                
                return View(dto);
            }
        }
    }
}

