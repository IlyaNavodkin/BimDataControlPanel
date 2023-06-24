using BimDataControlPanel.DAL.DbContexts;
using BimDataControlPanel.DAL.Entities;
using FluentValidation;

namespace BimDataControlPanel.DAL.Validators
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        private readonly AppDbContext _context;

        public ProjectValidator(AppDbContext context)
        {
            _context = context;
            RuleFor(item => item.Name)
                .NotEmpty().WithMessage("Имя не должно быть пустым.");

            RuleFor(item => item.CreationTime)
                .NotEmpty().WithMessage("Время создания проекта не должно быть пустым.");

            RuleFor(item => item.RevitVersion)
                .NotEmpty().WithMessage("Версия ревита проекта не должно быть пустым.");

            RuleFor(item => item.Complete)
                .NotNull().WithMessage("Укажите был ли проект завершен.");
            
            RuleFor(item => item)
                .Must(HaveUniqueChange)
                .WithMessage("Проект с похожим именем и версией уже есть в базе.");
        }
        
        private bool HaveUniqueChange(Project item)
        {
            var project = _context.Projects
                .FirstOrDefault(p => p.Name == item.Name && p.RevitVersion == item.RevitVersion);

            if (project is null) return true;
            if (item.Id == project.Id) return true;

            return false;
        }
        
        
    }
}
    