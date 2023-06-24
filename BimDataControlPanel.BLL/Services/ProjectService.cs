using BimDataControlPanel.BLL.Dtos.Project;
using BimDataControlPanel.DAL.DbContexts;
using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.DAL.Exeptions;
using BimDataControlPanel.DAL.Validators;
using Microsoft.EntityFrameworkCore;

namespace BimDataControlPanel.BLL.Services
{
    public class ProjectService
    {
        private readonly ProjectValidator _validator;
        private readonly AppDbContext _dataContext;

        public ProjectService(ProjectValidator validator, 
            AppDbContext dataContext)
        {
            _validator = validator;
            _dataContext = dataContext;
        }
        
        public async Task<List<Project>> GetAll()
        {
            return await _dataContext.Projects.ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetAllByUser(BimDataUser user)
        {
            var allProjects = await _dataContext.Projects.Include(p => p.ProjectUsers).ToListAsync();
            var projects = allProjects.Where(PredicateProjectByUserId(user));
        
            return projects;
        }

        private static Func<Project, bool> PredicateProjectByUserId(BimDataUser user)
        {
            return p =>
            {
                var argProjectUsers = p.ProjectUsers;

                foreach (var projectUser in argProjectUsers)
                {
                    if (projectUser.IdentityUserId == user.Id) return true;
                }

                return false;
            };
        }
    
        public async Task<Project> GetById(string? id)
        {
            var project = await _dataContext.Projects.FindAsync(id);
        
            if (project is null)
            {
                throw new NotFoundEntityException($"{nameof(Project)}");
            }

            return project;
        }
        
        public async Task<ProjectDto> GetDtoById(string? id)
        {
            var project = await GetById(id);
            
            var dto = new ProjectDto
            {
                Id = project.Id,
                RevitVersion = project.RevitVersion,
                Name = project.Name,
                CreationTime = project.CreationTime,
                Complete = project.Complete
            };
            
            return dto;
        }

        public async Task<Project?> GetByIdIncludeChanges(string id)
        {
            var project = await _dataContext.Projects
                .Include(p => p.Changes)
                .FirstOrDefaultAsync(p => p.Id == id);
        
            if (project is null)
            {
                throw new NotFoundEntityException($"{nameof(Project)}");
            }

            return project;
        }

        public async Task<Project?> GetByName(string name)
        {
            var project = await _dataContext.Projects.FirstOrDefaultAsync(p => p.Name == name);
        
            if (project is null)
            {
                throw new NotFoundEntityException($"{nameof(Project)}");
            }
            
            return project;
        }
        public void Update(Project changedItem)
        {
            _dataContext.Update(changedItem);
        }
        public void Save()
        {
            _dataContext.SaveChanges();
        }
        
        public async Task Create(CreateProjectDto model)
        {
            var projectNew = new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Complete = false,
                CreationTime = DateTime.Now,
                RevitVersion = model.RevitVersion
            };
            
            var validationResult = await _validator.ValidateAsync(projectNew);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            await _dataContext.Projects.AddAsync(projectNew);
            Save();
        }
        
        public async Task Edit(EditProjectDto dto)
        {
            var project = await GetById(dto.Id);
            
            project.Name = dto.Name;
            project.RevitVersion = dto.RevitVersion;
            project.Complete = dto.Complete;
            
            var result = await _validator.ValidateAsync(project);
            if (!result.IsValid) throw new ValidationException(result.Errors);
             
            Update(project);
            Save();
        }
        
        public async Task Delete(string? id)
        {
            var project = await GetById(id);

            _dataContext.Remove(project);
            Save();
        }
        
        public async Task<EditProjectDto> GetEditDto(string? id)
        {
            var project = await GetById(id);

            var editProjectDto = new EditProjectDto
            {
                Name = project.Name,
                RevitVersion = project.RevitVersion, 
                Id = project.Id, 
                Complete = project.Complete 
            };
            
            return editProjectDto;
        }
    }
}