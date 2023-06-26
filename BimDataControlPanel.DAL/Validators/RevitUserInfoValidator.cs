using BimDataControlPanel.DAL.DbContexts;
using BimDataControlPanel.DAL.Entities;
using FluentValidation;

namespace BimDataControlPanel.DAL.Validators
{
    public class RevitUserInfoValidator : AbstractValidator<RevitUserInfo>
    {
        private readonly AppDbContext _context;

        public RevitUserInfoValidator(AppDbContext context)
        {
            _context = context;
            RuleFor(item => item.Name)
                .NotEmpty().WithMessage("Имя не может быть пустым.");

            RuleFor(item => item.LastConnection)
                .NotEmpty().WithMessage("Последнее подключение не може тбыть пустым.");
            
            RuleFor(item => item.DsToolsVersion)
                .NotEmpty().WithMessage("Версия dstools не может быть пустым.");

            RuleFor(item => item.RevitVersion)
                .NotEmpty().WithMessage("Версия Revit не может быть пустым.");
            
            RuleFor(item => item.NameUserOs)
                .NotEmpty().WithMessage("Имя пользователя ОС не может быть пустым.");
            
            RuleFor(item => item)
                .Must(HaveUniqueChange)
                .WithMessage("Юзер инфо с похожими данными уже есть в базе.");
        }
        
        private bool HaveUniqueChange(RevitUserInfo item)
        {
            var userInfo = _context.RevitUserInfos
                .FirstOrDefault(p => p.Name == item.Name && p.RevitVersion == item.RevitVersion
                && p.NameUserOs == item.NameUserOs );
        
            if (userInfo is null) return true;
            if (item.Id == userInfo.Id) return true;
        
            return false;
        }
    }
}
    