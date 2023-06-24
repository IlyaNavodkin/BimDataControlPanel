using System.Text.RegularExpressions;
using BimDataControlPanel.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace BimDataControlPanel.DAL.Validators
{
    public class CustomPasswordValidator : IPasswordValidator<BimDataUser>
    {
        public int RequiredLength { get; set; } 

        public CustomPasswordValidator(int length)
        {
            RequiredLength = length;
        }

        public Task<IdentityResult> ValidateAsync(UserManager<BimDataUser> manager, BimDataUser user, string password)
        {
            var errors = new List<IdentityError>();

            if (String.IsNullOrEmpty(password) || password.Length < RequiredLength)
            {
                errors.Add(new IdentityError
                {
                    Description = $"Минимальная длина пароля равна {RequiredLength}"
                });
            }
            var pattern = "^[0-9]+$";

            if (!Regex.IsMatch(password, pattern))
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль должен состоять только из цифр"
                });
            }

            var identityErrors = errors.ToArray();
            
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(identityErrors));
        }
    }
}
