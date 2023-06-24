using BimDataControlPanel.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace BimDataControlPanel.DAL.Validators
{
    public class CustomUserValidator : IUserValidator<BimDataUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<BimDataUser> manager, BimDataUser user)
        {
            var errors = new List<IdentityError>();

            if (user.Email.ToLower().EndsWith("@spam.com"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Данный домен находится в спам-базе. Выберите другой почтовый сервис"
                });
            }
            if (user.UserName.Contains("admin"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Ник пользователя не должен содержать слово 'admin'"
                });
            }

            var identityErrors = errors.ToArray();
            
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(identityErrors));
        }
    }
}
