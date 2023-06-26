using BimDataControlPanel.BLL.Dtos.Change;
using BimDataControlPanel.DAL.DbContexts;
using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.DAL.Exeptions;
using BimDataControlPanel.DAL.Validators;
using Microsoft.EntityFrameworkCore;

namespace BimDataControlPanel.BLL.Services
{
    public class ChangeService
    {
        private readonly ChangeValidator _validator;
        private readonly AppDbContext _dataContext;

        public ChangeService(ChangeValidator validator, 
            AppDbContext dataContext)
        {
            _validator = validator;
            _dataContext = dataContext;
        }

        public async Task<List<Change>> GetAll()
        {
            var entities = await _dataContext.Changes.ToListAsync();

            return entities;
        }

        // public IEnumerable<Change> GetAllByUser(BimDataUser user)
        // {
        //     var entities = _dataContext.Changes
        //         .Where(c => c.UserRevitName == user.RevitUserNickName2020 || 
        //                     c.UserRevitName == user.RevitUserNickName2020);
        //
        //     return entities;
        // }

        // public IEnumerable<Change> GetAllByProject(Project project)
        // {
        //     var entities = _dataContext.Changes
        //         .Where(c => c.ProjectId == project.Id || c.UserRevitName == project.Id);
        //
        //     return entities;
        // }

        public async Task<Change?> GetById(string? id)
        {
            var entity = await _dataContext.Changes.FindAsync(id);
            if (entity is null) throw new NotFoundEntityException($"{nameof(Change)}");

            return entity;
        }

        public async Task<Change?> GetByIdIncludeProject(string id)
        {
            var entity = await _dataContext.Changes.Include(c => c.Project)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (entity is null) throw new NotFoundEntityException($"{nameof(Change)}");

            return entity;
        }

        public async Task<Change?> GetByDescription(string description)
        {
            var entity = await _dataContext.Changes.FirstOrDefaultAsync(c => c.Description == description);
            
            if (entity is null) throw new NotFoundEntityException($"{nameof(Change)}");

            return entity;
        }

        public void Update(Change changedItem)
        {
            _dataContext.Update(changedItem);
        }
        
        public void Save()
        {
            _dataContext.SaveChanges();
        }

        public async Task<string> Create(CreateChangeDto model)
        {
            var change = new Change
            {
                Id = Guid.NewGuid().ToString(),
                Description = model.Description,
                ChangeType = model.ChangeType,
                ChangeTime = model.ChangeTime,
                RevitUserInfoId = model.RevitUserInfoId,
                ProjectId = model.ProjectId,
            };
            
            var validationResult = await _validator.ValidateAsync(change);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            await _dataContext.Changes.AddAsync(change);
            Save();

            return change.Id;
        }
        
        public async Task Edit(ChangeDto dto)
        {
            var change = await GetById(dto.Id);

            change.ChangeTime = dto.ChangeTime;
            change.Description = dto.Description;
            change.ChangeType = dto.ChangeType;
            
            var validationResult = await _validator.ValidateAsync(change);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            Update(change);
            Save();
        }
        
        public async Task Delete(string? id)
        {
            var change = await GetById(id);

            _dataContext.Remove(change);
            Save();
        }
        
        public async Task<ChangeDto> GetChangeDto(string id)
        {
            var change = await GetByIdIncludeProject(id);
   
            var project = change.Project;

            var dto = new ChangeDto
            {
                Description = change.Description,
                ChangeTime = change.ChangeTime,
                ChangeType = change.ChangeType,
                Id = change.Id,
                ProjectId = project.Id
            };

            return dto;
        }
        
        public CreateChangeDto GetCreateDto(string projectId)
        {
            var model = new CreateChangeDto
            {
                ProjectId = projectId,
                ChangeTime = DateTime.Now
            };

            return model;
        }
    }
}