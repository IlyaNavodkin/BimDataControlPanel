using BimDataControlPanel.BLL.Dtos.Change;
using BimDataControlPanel.BLL.Dtos.RevitUserInfo;
using BimDataControlPanel.DAL.DbContexts;
using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.DAL.Exeptions;
using BimDataControlPanel.DAL.Validators;
using Microsoft.EntityFrameworkCore;

namespace BimDataControlPanel.BLL.Services
{
    public class RevitUserInfoService
    {
        private readonly RevitUserInfoValidator _validator;
        private readonly AppDbContext _dataContext;

        public RevitUserInfoService(RevitUserInfoValidator validator, 
            AppDbContext dataContext)
        {
            _validator = validator;
            _dataContext = dataContext;
        }

        public async Task<List<RevitUserInfo>> GetAll()
        {
            var entities = await _dataContext.RevitUserInfos.ToListAsync();

            return entities;
        }
        public async Task<RevitUserInfo?> GetById(string? id)
        {
            var entity = await _dataContext.RevitUserInfos.FindAsync(id);
            if (entity is null) throw new NotFoundEntityException($"{nameof(RevitUserInfo)}");

            return entity;
        }

        public void Update(RevitUserInfo changedItem)
        {
            _dataContext.Update(changedItem);
        }
        
        public void Save()
        {
            _dataContext.SaveChanges();
        }

        public async Task<string> Create(CreateRevitUserInfoDto model)
        {
            var entity = new RevitUserInfo
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                LastConnection = model.LastConnection,
                NameUserOs = model.NameUserOs,
                DsToolsVersion = model.DsToolsVersion,
                RevitVersion = model.RevitVersion,
            };
            
            var validationResult = await _validator.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            await _dataContext.RevitUserInfos.AddAsync(entity);
            Save();

            return entity.Id;
        }
        
        public async Task Edit(EditRevitUserInfoDto dto)
        {
            var entity = await GetById(dto.Id);

            entity.Name = dto.Name;
            entity.LastConnection = dto.LastConnection;
            entity.DsToolsVersion = dto.DsToolsVersion;
            entity.RevitVersion = dto.RevitVersion;
            entity.NameUserOs = dto.NameUserOs;
            
            var validationResult = await _validator.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            Update(entity);
            Save();
        }
        
        public async Task Delete(string? id)
        {
            var entity = await GetById(id);

            _dataContext.Remove(entity);
            Save();
        }
    }
}