using BimDataControlPanel.DAL.DbContexts;
using BimDataControlPanel.DAL.Entities;
using FluentValidation;

namespace BimDataControlPanel.DAL.Validators
{
    public class ChangeValidator : AbstractValidator<Change>
    {
        private readonly AppDbContext _context;

        public ChangeValidator(AppDbContext context)
        {
            _context = context;
            RuleFor(item => item.Description)
                .NotEmpty().WithMessage("Введите описание.");

            RuleFor(item => item.ChangeTime)
                .NotEmpty().WithMessage("Введите время создания.");
            
            RuleFor(item => item.ProjectId)
                .NotEmpty().WithMessage("ProjectId должен быть заполнен.");

            RuleFor(item => item.ChangeType)
                .NotEmpty().WithMessage("Тип изменения должен быть заполнен.");

            RuleFor(item => item)
                .Must(HaveUniqueChange)
                .WithMessage("Изменение с похожим описанием, типом и именем пользователя уже есть в базе.");
        }
        private bool HaveUniqueChange(Change item)
        {
            var change = _context.Changes
                .FirstOrDefault(p => p.Description == item.Description && p.ChangeTime == item.ChangeTime );
        
            if (change is null) return true;
            if (item.Id == change.Id) return true;
        
            return false;
        }
    }
}
    