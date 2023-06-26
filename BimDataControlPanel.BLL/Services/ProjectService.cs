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

        public async Task<IEnumerable<Project>> GetAllByUser(RevitUserInfo user)
        {
            var allEntities = await _dataContext.Projects
                .Include(p => p.RevitUserInfos)
                .ToListAsync();
            
            var entity = allEntities.Where(PredicateProjectByUserId(user));
        
            return entity;
        }

        private static Func<Project, bool> PredicateProjectByUserId(RevitUserInfo user)
        {
            return p =>
            {
                var entities = p.RevitUserInfos;

                foreach (var projectUser in entities)
                {
                    if (projectUser.Id == user.Id) return true;
                }

                return false;
            };
        }
    
        public async Task<Project> GetById(string? id)
        {
            var entity = await _dataContext.Projects.FindAsync(id);
        
            if (entity is null)
            {
                throw new NotFoundEntityException($"{nameof(Project)}");
            }

            return entity;
        }
        
        public async Task<ProjectDto> GetDtoById(string? id)
        {
            var entity = await GetById(id);
            
            var dto = new ProjectDto
            {
                Id = entity.Id,
                RevitVersion = entity.RevitVersion,
                Name = entity.Name,
                CreationTime = entity.CreationTime,
                Complete = entity.Complete
            };
            
            return dto;
        }

        public async Task<Project?> GetByIdIncludeChanges(string id)
        {
            var entity = await _dataContext.Projects
                .Include(p => p.Changes)
                .FirstOrDefaultAsync(p => p.Id == id);
        
            if (entity is null)
            {
                throw new NotFoundEntityException($"{nameof(Project)}");
            }

            return entity;
        }

        public async Task<Project?> GetByName(string name)
        {
            var entity = await _dataContext.Projects.FirstOrDefaultAsync(p => p.Name == name);
        
            if (entity is null)
            {
                throw new NotFoundEntityException($"{nameof(Project)}");
            }
            
            return entity;
        }
        public void Update(Project changedItem)
        {
            _dataContext.Update(changedItem);
        }
        public void Save()
        {
            _dataContext.SaveChanges();
        }
        
        public async Task<string> Create(CreateProjectDto model)
        {
            var entity = new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Complete = false,
                CreationTime = DateTime.Now,
                RevitVersion = model.RevitVersion
            };
            
            var validationResult = await _validator.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            await _dataContext.Projects.AddAsync(entity);
            Save();

            return entity.Id;
        }
        
        public async Task Edit(EditProjectDto dto)
        {
            var entity = await GetById(dto.Id);
            
            entity.Name = dto.Name;
            entity.RevitVersion = dto.RevitVersion;
            entity.Complete = dto.Complete;
            
            var result = await _validator.ValidateAsync(entity);
            if (!result.IsValid) throw new ValidationException(result.Errors);
             
            Update(entity);
            Save();
        }
        
        public async Task Delete(string? id)
        {
            var entity = await GetById(id);

            _dataContext.Remove(entity);
            Save();
        }
        
        public async Task<EditProjectDto> GetEditDto(string? id)
        {
            var entity = await GetById(id);

            var editProjectDto = new EditProjectDto
            {
                Name = entity.Name,
                RevitVersion = entity.RevitVersion, 
                Id = entity.Id, 
                Complete = entity.Complete 
            };
            
            return editProjectDto;
        }
    }
}