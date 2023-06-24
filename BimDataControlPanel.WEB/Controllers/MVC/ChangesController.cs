using BimDataControlPanel.BLL.Dtos.Change;
using BimDataControlPanel.BLL.Dtos.Project;
using BimDataControlPanel.BLL.Services;
using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.DAL.Exeptions;
using BimDataControlPanel.WEB.Constants;
using BimDataControlPanel.WEB.ViewModels.Pagination;
using BimDataControlPanel.WEB.ViewModels.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BimDataControlPanel.WEB.Controllers.MVC
{
    [Authorize(Roles = RolesConstants.AdminRoleName)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ChangesController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly ChangeService _changeService;

        public ChangesController(ProjectService projectService, 
            ChangeService changeService)
        {
            _projectService = projectService;
            _changeService = changeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id, int page = 1, string searchString = "",  string sortBy = "ChangeTime", string sortOrder = "asc")
        {
            ViewBag.ProjectId = id;
            var activeProject = await _projectService.GetByIdIncludeChanges(id);
            var changesQuery = activeProject.Changes.AsQueryable();
            
            changesQuery = SearchEntityBy(searchString, changesQuery);
            changesQuery = SortingEntityBy(sortBy, sortOrder, changesQuery); 
            
            int pageSize = 2;
            var changes = changesQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var pageInfo = new PageInfo { PageNumber=page, PageSize=pageSize, TotalItems=changesQuery.ToList().Count};

            var viewModel = new ProjectChangesViewModel
            {
                Project = new ProjectDto
                {
                    Name = activeProject.Name,
                    Complete = activeProject.Complete,
                    RevitVersion = activeProject.RevitVersion,
                    CreationTime = activeProject.CreationTime,
                    Id = activeProject.Id
                },
                Changes = changes,
                PageInfo = pageInfo,
                SearchString = searchString,
                SortBy = sortBy,
                SortOrder = sortOrder
            };
            
            return View(viewModel);
        }

        private IQueryable<Change> SearchEntityBy(string searchString, IQueryable<Change> changesQuery)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                changesQuery = changesQuery.Where(c => c.UserRevitName.Contains(searchString));
            }

            return changesQuery;
        }

        private IQueryable<Change> SortingEntityBy(string sortBy, string sortOrder, IQueryable<Change> changesQuery)
        {
            switch (sortBy)
            {
                case "ChangeTime":
                    changesQuery = sortOrder == "asc"
                        ? changesQuery.OrderBy(c => c.ChangeTime)
                        : changesQuery.OrderByDescending(c => c.ChangeTime);
                    break;
                case "ChangeType":
                    changesQuery = sortOrder == "asc"
                        ? changesQuery.OrderBy(c => c.ChangeType)
                        : changesQuery.OrderByDescending(c => c.ChangeType);
                    break;
                case "UserRevitName":
                    changesQuery = sortOrder == "asc"
                        ? changesQuery.OrderBy(c => c.UserRevitName)
                        : changesQuery.OrderByDescending(c => c.UserRevitName);
                    break;
                case "Description":
                    changesQuery = sortOrder == "asc"
                        ? changesQuery.OrderBy(c => c.Description)
                        : changesQuery.OrderByDescending(c => c.Description);
                    break;
                default:
                    changesQuery = changesQuery.OrderBy(c => c.ChangeTime);
                    break;
            }

            return changesQuery;
        }


        [HttpGet]
        public IActionResult Create(string projectId)
        {
            var dto = _changeService.GetCreateDto(projectId);
            
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateChangeDto model)
        {
            ModelState.Clear();
            await _changeService.Create(model);
        
            return RedirectToAction("Index", new { id = model.ProjectId });
            
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ModelState.Clear();
            var dto = await _changeService.GetChangeDto(id);
            
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChangeDto dto)
        {
            ModelState.Clear();
            await _changeService.Edit(dto);

            return RedirectToAction("Index", new { id = dto.ProjectId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string? id, string projId)
        {
            ModelState.Clear();
            await _changeService.Delete(id);
            
            return RedirectToAction("Index",new { id = projId });
        }
        
        [NonAction]
        private void GenerateModelStateErrors(Exception exception)
        {
            if (exception is ValidationException validationException)
            {
                foreach (var validationResultError in validationException.Errors)
                {
                    ModelState.AddModelError(validationResultError.PropertyName, validationResultError.ErrorMessage);
                }
            }
        }
    }
}

