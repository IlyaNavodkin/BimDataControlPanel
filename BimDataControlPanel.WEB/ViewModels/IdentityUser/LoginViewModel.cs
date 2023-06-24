using System.ComponentModel.DataAnnotations;

namespace BimDataControlPanel.WEB.ViewModels.IdentityUser
{
    public class LoginModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }
}
